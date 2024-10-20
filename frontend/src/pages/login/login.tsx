import React, { useState } from "react";
import styled from "styled-components";
import { GoogleLogin } from "@react-oauth/google";
import { useNavigate } from "react-router-dom";
import { useAuth } from "src/context/authContext";
import LoginIcon from "src/assets/images/loginIcon.svg";

const LoginWrapper = styled.section`
  background-color: ${({ theme }) => theme.background};
  color: ${({ theme }) => theme.white};
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const LoginForm = styled.form`
  display: flex;
  flex-grow: 1;
  flex-direction: column;
  align-items: center;
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  border-radius: 10px;
`;

const Icon = styled.img`
  width: 50px;
  margin-bottom: 20px;
`;

const Title = styled.h1`
  font-size: 24px;
  margin-bottom: 20px;
`;

const Input = styled.input`
  width: 100%;
  padding: 10px;
  margin-bottom: 15px;
  border: 1px solid #ddd;
  border-radius: 5px;
  font-size: 14px;

  &:focus {
    border-color: #4285f4;
    outline: none;
  }
`;

const LoginButton = styled.button`
  width: 100%;
  padding: 10px;
  background-color: ${({ theme }) => theme.button};
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;

  &:hover {
    background-color: #357ae8;
  }
`;

const Login: React.FC = () => {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSuccess = async (credentialResponse: any) => {
    try {
      const token = credentialResponse?.credential;

      if (!token) {
        throw new Error("No token provided");
      }

      const response = await fetch("/api/User/auth/google", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ Token: token }),
      });

      if (!response.ok) {
        throw new Error("Error sending token to backend");
      }

      const userData = await response.json();
      console.log("User Data from Backend:", userData);

      const companyCheckResponse = await fetch("/api/Company/hasCompany", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (!companyCheckResponse.ok) {
        throw new Error("Error checking company status");
      }

      const { hasCompany } = await companyCheckResponse.json();
      console.log(`Has Company: ${hasCompany}`);

      login(token, userData);
      if (hasCompany) {
        navigate("/home");
      } else {
        navigate("/dashboard");
      }
    } catch (error) {
      console.error("Error during login process:", error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = await fetch("/api/User/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) {
        throw new Error("Login failed. Please check your credentials.");
      }

      const userData = await response.json();
      console.log("User Data from Backend:", userData);

      const token = userData.token;
      login(token, userData);

      navigate("/home");
    } catch (error) {
      console.error("Error during login process:", error);
    } finally {
      setUsername("");
      setPassword("");
    }
  };

  return (
    <LoginWrapper>
      <LoginForm onSubmit={handleSubmit}>
        <Icon src={LoginIcon} alt="loginIcon" />
        <Title>Login to your Account</Title>
        <GoogleLogin
          onSuccess={handleSuccess}
          onError={() => console.log("Login Failed")}
        />
        <br />
        <Input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        <Input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <LoginButton type="submit">Login</LoginButton>
      </LoginForm>
    </LoginWrapper>
  );
};

export default Login;
