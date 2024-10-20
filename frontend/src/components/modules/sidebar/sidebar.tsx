import React, { useState } from "react";
import { NavLink } from "react-router-dom";
import styled from "styled-components";
import dashboard from "src/assets/icons/dashboard.svg";
import calendar from "src/assets/icons/calendar.svg";
import contracts from "src/assets/icons/contracts.svg";
import archive from "src/assets/icons/stats.svg";
import logout from "src/assets/icons/logout.svg";
import sidebar from "src/assets/icons/sidebar.svg";
import logo from "src/assets/images/logo.svg";

interface NavigationProps {
  isMobile: boolean;
  isSidebarVisible: boolean;
}
interface SidebarProps {
  isMobile: boolean;
  isSidebarVisible: boolean;
}

const Background = styled.section`
  height: 100vh;
  background-color: ${({ theme }) => theme.background};
  color: ${({ theme }) => theme.white};
  display: flex;
`;

const Navigation = styled.nav<NavigationProps>`
  width: ${({ isSidebarVisible }) => (isSidebarVisible ? "300px" : "60px")};
  transition: width 0.3s ease;
  background-color: ${({ theme }) => theme.navBackground};
  display: flex;
  flex-direction: column;
  padding: 10px;
  align-items: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
`;

const Navbar = styled.ul`
  display: flex;
  flex-direction: column;
  gap: 1rem;
  width: 100%;
  align-items: center;
`;

const ListItem = styled.li<{ isSidebarVisible: boolean }>`
  display: flex;
  gap: 1rem;
  align-items: center;
  justify-content: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
  padding: 10px;
  width: 100%;
  cursor: pointer;

  &:hover {
    background-color: ${({ theme }) => theme.hoverBackground};
  }
`;

const ListItemIcons = styled.img`
  width: 24px;
`;

const StyledNavLink = styled(NavLink)<{
  isSidebarVisible?: boolean;
}>`
  color: ${({ theme }) => theme.white};
  display: flex;
  align-items: center;
  justify-content: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
  text-decoration: none;
  padding: ${({ isSidebarVisible }) => (isSidebarVisible ? "10px" : "20px")};
  width: 100%;

  &.active {
    background-color: #fff;
    border-radius: 25px;
    color: #000;

    img {
      filter: invert(100%);
    }
  }

  span {
    display: ${({ isSidebarVisible }) =>
      isSidebarVisible ? "inline-block" : "none"};
    margin-left: 10px;
    transition: display 0.3s ease;
  }
`;

const SidebarIcon = styled.img<{ isSidebarVisible: boolean }>`
  cursor: pointer;
  display: flex;
  align-self: flex-end;
  width: 24px;
`;

const Logo = styled.img<{ isSidebarVisible?: boolean }>`
  width: 70%;
  transition: opacity 0.3s ease;
  opacity: ${({ isSidebarVisible }) => (isSidebarVisible ? 1 : 0)};
  display: ${({ isSidebarVisible }) => (isSidebarVisible ? "block" : "none")};
`;

const Sidebar: React.FC<SidebarProps> = ({ isMobile }) => {
  const [sidebarVisible, setSidebarVisible] = useState(true);

  const toggleSidebar = () => {
    setSidebarVisible((prevState: boolean) => !prevState);
  };

  return (
    <Background>
      <Navigation isMobile={isMobile} isSidebarVisible={sidebarVisible}>
        <Navbar>
          <ListItem isSidebarVisible={sidebarVisible}>
            <Logo src={logo} alt="logo" isSidebarVisible={sidebarVisible} />
            <SidebarIcon
              src={sidebar}
              alt="sidebar"
              onClick={toggleSidebar}
              isSidebarVisible={sidebarVisible}
            />
          </ListItem>
          <ListItem isSidebarVisible={sidebarVisible}>
            <StyledNavLink
              to="/dashboard"
              isSidebarVisible={sidebarVisible}
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              <ListItemIcons src={dashboard} alt="dashboard" />
              <span>Dashboard</span>
            </StyledNavLink>
          </ListItem>
          <ListItem isSidebarVisible={sidebarVisible}>
            <StyledNavLink
              to="/contractors"
              isSidebarVisible={sidebarVisible}
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              <ListItemIcons src={contracts} alt="contracts" />
              <span>Kontrahenci</span>
            </StyledNavLink>
          </ListItem>
          <ListItem isSidebarVisible={sidebarVisible}>
            <StyledNavLink
              to="/stats"
              isSidebarVisible={sidebarVisible}
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              <ListItemIcons src={"stats"} alt="stats" />
              <span>Zgłoszenia</span>
            </StyledNavLink>
          </ListItem>
          <ListItem isSidebarVisible={sidebarVisible}>
            <StyledNavLink
              to="/archive"
              isSidebarVisible={sidebarVisible}
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              <ListItemIcons src={archive} alt="archive" />
              <span>Archiwum</span>
            </StyledNavLink>
          </ListItem>
          <ListItem isSidebarVisible={sidebarVisible}>
            <StyledNavLink
              to="/calendar"
              isSidebarVisible={sidebarVisible}
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              <ListItemIcons src={calendar} alt="calendar" />
              <span>Kalendarz</span>
            </StyledNavLink>
          </ListItem>
          <ListItem isSidebarVisible={sidebarVisible}>
            <StyledNavLink
              to="/logout"
              isSidebarVisible={sidebarVisible}
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              <ListItemIcons src={logout} alt="logout" />
              <span>Wyloguj</span>
            </StyledNavLink>
          </ListItem>
        </Navbar>
      </Navigation>
    </Background>
  );
};

export default Sidebar;
