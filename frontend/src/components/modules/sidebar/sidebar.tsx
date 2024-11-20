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

const Navigation = styled.nav<{ isSidebarVisible: boolean }>`
  width: ${({ isSidebarVisible }) => (isSidebarVisible ? "300px" : "60px")};
  transition: width 0.3s ease;
  background-color: ${({ theme }) => theme.navBackground};
  display: flex;
  flex-direction: column;
  padding: 10px;
  align-items: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
  overflow: hidden;
`;

const Navbar = styled.ul`
  display: flex;
  flex-direction: column;
  gap: 1rem;
  width: 100%;
  align-items: center;
`;

const ListItem = styled.li<{ isActive: boolean; isSidebarVisible: boolean }>`
  display: flex;
  gap: ${({ isSidebarVisible }) => (isSidebarVisible ? "1rem" : "0")};
  align-items: center;
  justify-content: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
  padding: 10px;
  width: 100%;
  height: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "auto" : "60px"}; // Wysokość w stanie zwiniętym
  cursor: pointer;
  background-color: ${({ isActive, theme }) =>
    isActive ? theme.white : "transparent"};
  border-radius: 50px;

  &:hover {
    background-color: ${({ theme }) => theme.hoverBackground};
  }
`;

const ListItemIcons = styled.img<{ isSidebarVisible: boolean }>`
  width: 24px;
  display: block;
  margin: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "0" : "auto"}; // Wyśrodkowanie ikony w trybie zwiniętym
`;

const StyledNavLink = styled(NavLink)<{ isSidebarVisible?: boolean }>`
  color: ${({ theme }) => theme.white};
  display: flex;
  align-items: center;
  justify-content: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "flex-start" : "center"};
  text-decoration: none;
  padding: ${({ isSidebarVisible }) => (isSidebarVisible ? "10px" : "0")};
  width: 100%;
  height: ${({ isSidebarVisible }) =>
    isSidebarVisible ? "auto" : "60px"}; // Ustaw wysokość linku

  &.active {
    background-color: ${({ theme, isSidebarVisible }) =>
      isSidebarVisible
        ? "transparent"
        : theme.white}; // Aktywne tło w stanie zwiniętym
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

const SidebarIcon = styled.img`
  cursor: pointer;
  width: 24px;
  align-self: flex-end;
`;

const Logo = styled.img<{ isSidebarVisible?: boolean }>`
  width: 70%;
  transition: opacity 0.3s ease;
  opacity: ${({ isSidebarVisible }) => (isSidebarVisible ? 1 : 0)};
  display: ${({ isSidebarVisible }) => (isSidebarVisible ? "block" : "none")};
`;

const Sidebar: React.FC<SidebarProps> = ({ isMobile, isSidebarVisible }) => {
  const [isSidebarOpen, setSidebarOpen] = React.useState(isSidebarVisible);
  const location = useLocation();

  const toggleSidebar = () => {
    setSidebarOpen((prevState) => !prevState);
  };

  return (
    <Background>
      <Navigation isSidebarVisible={isSidebarOpen}>
        <Navbar>
          <ListItem isActive={false} isSidebarVisible={isSidebarOpen}>
            <Logo src={logo} alt="logo" isSidebarVisible={isSidebarOpen} />
            <SidebarIcon
              src={sidebar}
              alt="toggle sidebar"
              onClick={toggleSidebar}
            />
          </ListItem>
          <ListItem
            isActive={location.pathname === "/home"}
            isSidebarVisible={isSidebarOpen}
          >
            <StyledNavLink to="/home" isSidebarVisible={isSidebarOpen}>
              <ListItemIcons
                src={dashboard}
                alt="home"
                isSidebarVisible={isSidebarOpen}
              />
              <span>Strona główna</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isActive={location.pathname === "/contractors"}
            isSidebarVisible={isSidebarOpen}
          >
            <StyledNavLink to="/contractors" isSidebarVisible={isSidebarOpen}>
              <ListItemIcons
                src={contracts}
                alt="contracts"
                isSidebarVisible={isSidebarOpen}
              />
              <span>Kontrahenci</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isActive={location.pathname === "/stats"}
            isSidebarVisible={isSidebarOpen}
          >
            <StyledNavLink to="/stats" isSidebarVisible={isSidebarOpen}>
              <ListItemIcons
                src={stats}
                alt="stats"
                isSidebarVisible={isSidebarOpen}
              />
              <span>Zgłoszenia</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isActive={location.pathname === "/archive"}
            isSidebarVisible={isSidebarOpen}
          >
            <StyledNavLink to="/archive" isSidebarVisible={isSidebarOpen}>
              <ListItemIcons
                src={archive}
                alt="archive"
                isSidebarVisible={isSidebarOpen}
              />
              <span>Archiwum</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isActive={location.pathname === "/calendar"}
            isSidebarVisible={isSidebarOpen}
          >
            <StyledNavLink to="/calendar" isSidebarVisible={isSidebarOpen}>
              <ListItemIcons
                src={calendar}
                alt="calendar"
                isSidebarVisible={isSidebarOpen}
              />
              <span>Kalendarz</span>
            </StyledNavLink>
          </ListItem>
          <ListItem
            isActive={location.pathname === "/logout"}
            isSidebarVisible={isSidebarOpen}
          >
            <StyledNavLink to="/logout" isSidebarVisible={isSidebarOpen}>
              <ListItemIcons
                src={logout}
                alt="logout"
                isSidebarVisible={isSidebarOpen}
              />
              <span>Wyloguj</span>
            </StyledNavLink>
          </ListItem>
        </Navbar>
      </Navigation>
    </Background>
  );
};

export default Sidebar;
