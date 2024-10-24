import React from "react";
import { NavLink, useLocation } from "react-router-dom";
import styled from "styled-components";
import dashboard from "src/assets/icons/dashboard.svg";
import calendar from "src/assets/icons/calendar.svg";
import contracts from "src/assets/icons/contracts.svg";
import stats from "src/assets/icons/stats.svg";
import archive from "src/assets/icons/archive.svg";
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

const ListItem = styled.li<{ isSidebarVisible: boolean; isActive: boolean }>`
  display: flex;
  gap: 1rem;
  align-items: center;
  justify-content: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
  padding: 10px;
  width: 100%;
  cursor: pointer;
  background-color: ${({ isActive, theme }) =>
    isActive ? theme.white : "transparent"};
  border-radius: 50px;
  &:hover {
    background-color: ${({ theme }) => theme.hoverBackground};
  }
`;

const ListItemIcons = styled.img`
  width: 24px;
`;

const StyledNavLink = styled(NavLink)<{ isSidebarVisible?: boolean }>`
  color: ${({ theme }) => theme.white};
  display: flex;
  align-items: center;
  justify-content: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
  text-decoration: none;
  padding: ${({ isSidebarVisible }) => (isSidebarVisible ? "10px" : "20px")};
  width: 100%;

  &.active {
    background-color: transparent;
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

const Sidebar: React.FC<SidebarProps> = ({ isMobile, isSidebarVisible }) => {
  const location = useLocation();

  return (
    <Background>
      <Navigation isMobile={isMobile} isSidebarVisible={isSidebarVisible}>
        <Navbar>
          <ListItem isSidebarVisible={isSidebarVisible} isActive={false}>
            <Logo src={logo} alt="logo" isSidebarVisible={isSidebarVisible} />
            <SidebarIcon
              src={sidebar}
              alt="sidebar"
              isSidebarVisible={isSidebarVisible}
            />
          </ListItem>
          <ListItem
            isSidebarVisible={isSidebarVisible}
            isActive={location.pathname === "/home"}
          >
            <StyledNavLink to="/home" isSidebarVisible={isSidebarVisible}>
              <ListItemIcons src={dashboard} alt="home" />
              <span>Strona główna</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isSidebarVisible={isSidebarVisible}
            isActive={location.pathname === "/contractors"}
          >
            <StyledNavLink
              to="/contractors"
              isSidebarVisible={isSidebarVisible}
            >
              <ListItemIcons src={contracts} alt="contracts" />
              <span>Kontrahenci</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isSidebarVisible={isSidebarVisible}
            isActive={location.pathname === "/stats"}
          >
            <StyledNavLink to="/stats" isSidebarVisible={isSidebarVisible}>
              <ListItemIcons src={stats} alt="stats" />
              <span>Zgłoszenia</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isSidebarVisible={isSidebarVisible}
            isActive={location.pathname === "/archive"}
          >
            <StyledNavLink to="/archive" isSidebarVisible={isSidebarVisible}>
              <ListItemIcons src={archive} alt="archive" />
              <span>Archiwum</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isSidebarVisible={isSidebarVisible}
            isActive={location.pathname === "/calendar"}
          >
            <StyledNavLink to="/calendar" isSidebarVisible={isSidebarVisible}>
              <ListItemIcons src={calendar} alt="calendar" />
              <span>Kalendarz</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isSidebarVisible={isSidebarVisible}
            isActive={location.pathname === "/logout"}
          >
            <StyledNavLink to="/logout" isSidebarVisible={isSidebarVisible}>
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
