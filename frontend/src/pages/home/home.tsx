import React, { useState, useEffect } from "react";
import styled from "styled-components";
import { useAuth } from "../../context/authContext";
import dashboard from "../../assets/icons/dashboard.svg";
import calendar from "../../assets/icons/calendar.svg";
import contracts from "../../assets/icons/contracts.svg";
import archive from "../../assets/icons/archive.svg";
import stats from "../../assets/icons/stats.svg";
import logout from "../../assets/icons/logout.svg";
import sidebar from "../../assets/icons/sidebar.svg";
import logo from "../../assets/images/logo.svg";

// Define props for the Navigation styled component
interface NavigationProps {
  isMobile: boolean;
  isSidebarVisible: boolean;
}

// Styled components
const Background = styled.section`
  height: 100vh;
  background-color: ${({ theme }) => theme.background};
  color: ${({ theme }) => theme.white};
  display: flex;
`;

const Navigation = styled.nav<NavigationProps>`
  width: ${({ isMobile, isSidebarVisible }) =>
    isMobile || isSidebarVisible ? "60px" : "200px"};
  transition: width 0.3s ease;
  background-color: ${({ theme }) => theme.navBackground};
  display: flex;
  flex-direction: column;
  padding: 10px;
`;

const Navbar = styled.ul`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;

const ListItem = styled.li<{ active?: boolean }>`
  display: flex;
  gap: 0.5rem;
  align-items: center;
  padding: 10px;
  background-color: ${({ active }) => (active ? "#fff" : "transparent")};
  color: ${({ active }) => (active ? "#000" : "#fff")};
  cursor: pointer;

  img {
    width: 24px;
    filter: ${({ active }) => (active ? "invert(100%)" : "none")};
  }

  &:hover {
    background-color: ${({ theme }) => theme.hoverBackground};
  }
`;

const StyledLink = styled.a<{ isMobile?: boolean; isSidebarVisible?: boolean }>`
  color: ${({ theme }) => theme.white};
  display: ${({ isMobile, isSidebarVisible }) =>
    isMobile || isSidebarVisible ? "none" : "inline-block"};
  transition: display 0.3s ease;

  @media (min-width: 768px) {
    display: ${({ isSidebarVisible }) =>
      isSidebarVisible ? "none" : "inline-block"};
  }
`;

const SidebarIcon = styled.img`
  cursor: pointer;
`;

const Home = () => {
  const { user } = useAuth();
  const [isSidebarVisible, setSidebarVisible] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth <= 768);

  // Handle window resize to detect mobile view
  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768);
    };

    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  const toggleSidebar = () => {
    setSidebarVisible((prevState) => !prevState);
  };

  return (
    <Background>
      <Navigation isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
        <Navbar>
          <ListItem>
            {!isSidebarVisible && (
              <img style={{ width: "70%" }} src={logo} alt="logo" />
            )}
            {!isMobile && (
              <SidebarIcon
                src={sidebar}
                alt="sidebar"
                onClick={toggleSidebar}
              />
            )}
          </ListItem>
          <ListItem active>
            <img src={dashboard} alt="dashboard" />
            <StyledLink isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
              Dashboard
            </StyledLink>
          </ListItem>
          <ListItem>
            <img src={contracts} alt="contracts" />
            <StyledLink isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
              Kontrahenci
            </StyledLink>
          </ListItem>
          <ListItem>
            <img src={stats} alt="stats" />
            <StyledLink isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
              Zgłoszenia
            </StyledLink>
          </ListItem>
          <ListItem>
            <img src={archive} alt="archive" />
            <StyledLink isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
              Archiwum
            </StyledLink>
          </ListItem>
          <ListItem>
            <img src={calendar} alt="calendar" />
            <StyledLink isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
              Kalendarz
            </StyledLink>
          </ListItem>
          <ListItem>
            <img src={calendar} alt="notes" />
            <StyledLink isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
              Notatki
            </StyledLink>
          </ListItem>
          <ListItem>
            <img src={logout} alt="logout" />
            <StyledLink isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
              Wyloguj
            </StyledLink>
          </ListItem>
        </Navbar>
      </Navigation>
      <div>Home - {user?.company?.name}</div>
    </Background>
  );
};

export default Home;
