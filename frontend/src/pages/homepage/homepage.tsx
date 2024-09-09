import React, { useEffect, useState } from "react";
import styled from "styled-components";
import Logo from "../../assets/images/logo.svg";
import Youtube from "../../assets/images/youtube.svg";
import X from "../../assets/images/x.svg";
import Instagram from "../../assets/images/instagram.svg";
import axios from "axios";

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
  const [email, setEmail] = useState<string | null>(null);

  // useEffect(() => {
  //   const fetchData = async () => {
  //     try {
  //       let token = localStorage.getItem("authToken");

  //       if (!token) {
  //         alert("You are not logged in. Please log in to continue.");
  //         window.location.href = "/login"; // Replace with your login route
  //         return;
  //       }

  //       const response = await axios.get(
  //         "https://localhost:7234/api/User/signin-google",
  //         {
  //           headers: {
  //             Authorization: `Bearer ${token}`,
  //           },
  //         }
  //       );

  //       const { claims } = response.data;
  //       console.log("Claims:", claims);

  //       const emailClaim = claims.find(
  //         (claim: { type: string }) =>
  //           claim.type ===
  //           "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
  //       );

  //       if (emailClaim) {
  //         setEmail(emailClaim.value);
  //       } else {
  //         setEmail(null);
  //       }
  //     } catch (error) {
  //       if (axios.isAxiosError(error)) {
  //         console.error(
  //           "Error fetching data:",
  //           error.response?.data || error.message
  //         );
  //       } else {
  //         console.error("Error:", error);
  //       }
  //     }
  //   };

  //   fetchData();
  // }, []);

  return (
    <HomepageWrapper>
      <CenteredContent>
        <img src={Logo} alt="devCenterLogo" />
        <HomepageHeading>
          Wejdź do świata innowacji <br />
          <InnerHeading>Rozwiń swój biznes razem z nami</InnerHeading>
        </HomepageHeading>
        {email && <p>Email: {email}</p>} {/* Display the email if available */}
      </CenteredContent>
      <IconWrapper>
        <img src={Youtube} alt="Youtube Icon" />
        <img src={X} alt="Twitter Icon" />
        <img src={Instagram} alt="Instagram Icon" />
      </IconWrapper>
    </HomepageWrapper>
  );
};

export default Homepage;
