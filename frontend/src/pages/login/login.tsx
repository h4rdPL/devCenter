import styled from "styled-components";
import Google from "../../assets/images/google.svg";
import LoginIcon from "../../assets/images/loginIcon.svg";

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
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
`;

const Icon = styled.img`
  width: 50px;
  margin-bottom: 20px;
`;

const Title = styled.h1`
  font-size: 24px;
  margin-bottom: 20px;
`;

const GoogleButton = styled.button`
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  padding: 10px;
  margin-bottom: 15px;
  background-color: transparent;
  color: white;
  border: 1px solid ${({ theme }) => theme.white};
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;

  &:hover {
    border: 1px solid ${({ theme }) => theme.lightBlue};
    background-color: #357ae8;
  }
`;

const GoogleIcon = styled.img`
  width: 20px;
  margin-right: 10px;
`;

const Text = styled.p`
  font-size: 14px;
  margin-bottom: 10px;
`;

const Label = styled.label`
  font-size: 14px;
  color: ${({ theme }) => theme.white};
  align-self: flex-start;
  margin-bottom: 5px;
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

const Options = styled.div`
  display: flex;
  justify-content: space-between;
  width: 100%;
  margin-bottom: 20px;
`;

const RememberMe = styled.span`
  display: flex;
  gap: 0.25rem;
  font-size: 14px;
  cursor: pointer;
  &:hover {
    text-decoration: underline;
  }
`;

const ForgotPassword = styled.span`
  font-size: 14px;
  color: ${({ theme }) => theme.warning};
  font-weight: 700;
  cursor: pointer;

  &:hover {
    text-decoration: underline;
  }
`;

const RegisterPrompt = styled.p`
  font-size: 14px;
  color: ${({ theme }) => theme.white};
  font-weight: bold;
  margin-bottom: 20px;
  align-self: flex-start;
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
  return (
    <LoginWrapper>
      <LoginForm>
        <Icon src={LoginIcon} alt="loginIcon" />
        <Title>Login to your Account</Title>
        <GoogleButton>
          <GoogleIcon src={Google} alt="Google icon" />
          Continue with Google
        </GoogleButton>
        <Text>Or Sign in with Email address</Text>
        <Label htmlFor="email">Email</Label>
        <Input type="text" placeholder="Email" />
        <Label htmlFor="password">Password</Label>
        <Input type="text" placeholder="Password" />
        <Options>
          <RememberMe>
            <input type="checkbox" />
            Remember me?
          </RememberMe>
          <ForgotPassword>Forgot password</ForgotPassword>
        </Options>
        <RegisterPrompt>Don't have an account? Register now!</RegisterPrompt>
        <LoginButton>Login</LoginButton>
      </LoginForm>
    </LoginWrapper>
  );
};

export default Login;