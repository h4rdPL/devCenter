import styled from "styled-components";
import Logo from "../../assets/images/logo.svg";
import Youtube from "../../assets/images/youtube.svg";
import X from "../../assets/images/x.svg";
import Instagram from "../../assets/images/instagram.svg";

const HomepageWrapper = styled.section`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  background-color: ${({ theme }) => theme.background};
  min-height: 100vh;
  padding: 0 2rem;
`;

const CenteredContent = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 4rem;
  flex-grow: 1;
  justify-content: center;
`;

const HomepageHeading = styled.h1`
  color: ${({ theme }) => theme.white};
  text-align: center;
`;

const InnerHeading = styled.span`
  color: ${({ theme }) => theme.lightBlue};
  font-size: 1.3rem;
`;

const IconWrapper = styled.div`
  display: flex;
  gap: 1.5rem;
  margin-top: auto;
  padding-bottom: 2rem;
`;

const Homepage: React.FC = () => {
  return (
    <HomepageWrapper>
      <CenteredContent>
        <img src={Logo} alt="devCenterLogo" />
        <HomepageHeading>
          Wejdź do świata innowacji <br />
          <InnerHeading>Rozwiń swój biznes razem z nami</InnerHeading>
        </HomepageHeading>
      </CenteredContent>
      <IconWrapper>
        <img src={Youtube} alt="Youtube Icon" />
        <img src={X} alt="Youtube Icon" />
        <img src={Instagram} alt="Youtube Icon" />
      </IconWrapper>
    </HomepageWrapper>
  );
};

export default Homepage;