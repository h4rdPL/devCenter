import React from "react";
import Sidebar from "src/components/modules/sidebar/sidebar";
import styled from "styled-components";

interface LayoutProps {
  children: React.ReactNode;
}

const Container = styled.section`
  display: flex;
  flex-direction: row;
  background-color: ${({ theme }) => theme.background};
`;

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <Container>
      <Sidebar isMobile={false} isSidebarVisible={true} />
      {children}
    </Container>
  );
};
