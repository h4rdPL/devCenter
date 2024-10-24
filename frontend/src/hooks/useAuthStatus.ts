import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "src/context/authContext";
import { jwtDecode } from "jwt-decode";

const isTokenExpired = (token: string): boolean => {
  const decodedToken: any = jwtDecode(token);
  const currentTime = Date.now() / 1000;
  return decodedToken.exp < currentTime;
};

const useAuthStatus = () => {
  const { isAuthenticated, user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (!token || isTokenExpired(token)) {
      navigate("/login");
    }
  }, [navigate]);

  return { isAuthenticated, user };
};

export default useAuthStatus;
