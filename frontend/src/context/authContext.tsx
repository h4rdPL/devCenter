import {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";
import { jwtDecode } from "jwt-decode";

interface Company {
  id: string;
  name: string;
  nip: string;
  country: string;
  city: string;
  postalCode: string;
  street: string;
  companyEmail: string;
}

interface User {
  id: string;
  username: string;
  email: string;
  picture: string;
  password?: string;
  role?: string;
  company?: Company | null;
}

interface AuthContextType {
  isAuthenticated: boolean;
  user: User | null;
  login: (token: string, userData: User) => void;
  logout: () => void;
  refreshToken: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const isTokenExpired = (token: string): boolean => {
  const decodedToken: any = jwtDecode(token);
  const currentTime = Date.now() / 1000;
  return decodedToken.exp < currentTime;
};

const requestNewToken = async (): Promise<string> => {
  const response = await fetch("/api/auth/refresh-token", {
    method: "POST",
    credentials: "include",
  });

  if (!response.ok) {
    throw new Error("Failed to refresh token");
  }

  const data = await response.json();
  return data.token;
};

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("token")
  );
  const isAuthenticated = !!user;

  const login = (newToken: string, userData: User) => {
    localStorage.setItem("token", newToken);
    setToken(newToken);
    setUser(userData);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setUser(null);
    setToken(null);
  };

  const refreshToken = async () => {
    try {
      const newToken = await requestNewToken();
      localStorage.setItem("token", newToken);
      setToken(newToken);
    } catch (error) {
      console.error("Error refreshing token:", error);
      logout();
    }
  };

  useEffect(() => {
    if (token) {
      const decodedToken: any = jwtDecode(token);
      const expirationTime = decodedToken.exp * 1000 - 60000; // 1 minute before expiration

      const timeoutId = setTimeout(() => {
        refreshToken();
      }, expirationTime - Date.now());

      return () => clearTimeout(timeoutId); // Clear timeout on cleanup
    }
  }, [token]);

  useEffect(() => {
    if (token && isTokenExpired(token)) {
      refreshToken();
    }
  }, [token]);

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, user, login, logout, refreshToken }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
