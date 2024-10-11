import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import styled, { keyframes } from "styled-components";
import Confetti from "react-confetti";
import { useWindowSize } from "react-use";

const DashboardWrapper = styled.section`
  background-color: ${({ theme }) => theme.background};
  color: ${({ theme }) => theme.white};
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 20px;
  position: relative;
`;

const DashboardContent = styled.div`
  text-align: center;
  max-width: 600px;
  width: 100%;
  background-color: ${({ theme }) => theme.cardBackground};
  padding: 40px;
  border-radius: 10px;
  box-shadow: 0 2px 20px rgba(0, 0, 0, 0.2);
  position: relative;
  z-index: 1; /* Ensure content appears above confetti */
`;

const Title = styled.h1`
  font-size: 28px;
  color: ${({ theme }) => theme.primary};
  margin-bottom: 20px;
  text-align: center;
`;

const Button = styled.button`
  width: 100%;
  padding: 12px;
  background-color: ${({ theme }) => theme.button};
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  margin-top: 10px;

  &:hover {
    background-color: ${({ theme }) => theme.buttonHover};
  }
`;

const FormTitle = styled.h3`
  font-size: 22px;
  margin-bottom: 20px;
  text-align: center;
`;

const FormInput = styled.input`
  width: 100%;
  padding: 12px;
  margin-bottom: 15px;
  border: 1px solid #ddd;
  border-radius: 5px;
  font-size: 16px;

  &:focus {
    border-color: ${({ theme }) => theme.primary};
    outline: none;
  }
`;

const Form = styled.form`
  display: flex;
  flex-direction: column;
  gap: 15px;
  transition: opacity 0.5s ease-in-out, transform 0.5s ease-in-out;
`;

const Image = styled.img`
  width: 100px;
  height: 100px;
  border-radius: 50%;
  object-fit: cover;
  margin-bottom: 20px;
`;

const LogoutButton = styled(Button)`
  background-color: ${({ theme }) => theme.warning};
  &:hover {
    background-color: ${({ theme }) => theme.warningHover};
  }
`;

const fadeIn = keyframes`
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
`;

const StepIndicator = styled.div`
  display: flex;
  justify-content: center;
  margin-bottom: 20px;
  animation: ${fadeIn} 0.5s ease-in-out;
`;

const Step = styled.div<{ active: boolean }>`
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background-color: ${({ active, theme }) =>
    active ? theme.primary : theme.white};
  color: ${({ active, theme }) => (active ? theme.white : theme.background)};
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 10px;
  font-weight: bold;
  border: 2px solid ${({ active, theme }) => (active ? theme.primary : "#ccc")};
`;

const ErrorMessage = styled.p`
  color: #ff4d4f;
  font-size: 14px;
  margin-bottom: 10px;
`;

const SuccessMessage = styled.p`
  color: #52c41a;
  font-size: 14px;
  margin-bottom: 10px;
`;

const Dashboard = () => {
  const [company, setCompany] = useState({
    name: "",
    nip: "",
    country: "",
    city: "",
    postalCode: "",
    street: "",
    email: "",
  });
  const [currentStep, setCurrentStep] = useState(1);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [showConfetti, setShowConfetti] = useState(false);
  const token = localStorage.getItem("token");

  const location = useLocation();
  const navigate = useNavigate();
  const userData = location.state?.userData;
  console.log(userData);

  const { width, height } = useWindowSize();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setCompany({ ...company, [name]: value });
  };

  const handleSubmitCompany = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);
    setSuccess(null);

    if (company.nip.length !== 10 || !/^\d{10}$/.test(company.nip)) {
      setError("NIP must be exactly 10 digits.");
      return;
    }

    try {
      const response = await fetch(`https://localhost:7234/api/Company/add`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          ...company,
          companyEmail: userData.email,
        }),
      });

      if (response.ok) {
        console.log("Company added successfully.");
        setSuccess("Company added successfully.");
        setCompany({
          name: "",
          nip: "",
          country: "",
          city: "",
          postalCode: "",
          street: "",
          email: userData.email,
        });
        setCurrentStep(1);
        setShowConfetti(true);
        setTimeout(() => {
          setShowConfetti(false);
        }, 5000);
      } else {
        const errorData = await response.json();
        console.error("Failed to add company:", errorData);
        setError(errorData.message || "Failed to add company.");
      }
    } catch (error) {
      console.error("Error:", error);
      setError("An unexpected error occurred.");
    }
  };

  const nextStep = () => {
    setError(null);
    setSuccess(null);
    setCurrentStep((prev) => prev + 1);
  };

  const prevStep = () => {
    setError(null);
    setSuccess(null);
    setCurrentStep((prev) => prev - 1);
  };

  const imageUrl = userData.picture || "https://via.placeholder.com/100";

  return (
    <DashboardWrapper>
      {/* Confetti Animation in Fullscreen */}
      {showConfetti && <Confetti width={width} height={height} />}

      <DashboardContent>
        <Title>
          Welcome, <br /> {userData.email}
        </Title>
        <Image
          src={imageUrl}
          alt={userData.name}
          onError={(e) => {
            e.currentTarget.src = "https://via.placeholder.com/100";
          }}
        />

        <StepIndicator>
          <Step active={currentStep === 2}>1</Step>
          <Step active={currentStep === 1}>2</Step>
        </StepIndicator>

        {error && <ErrorMessage>{error}</ErrorMessage>}
        {success && <SuccessMessage>{success}</SuccessMessage>}

        {currentStep === 1 && (
          <>
            <FormTitle>Step 1: Company Details</FormTitle>
            <Form onSubmit={(e) => e.preventDefault()}>
              <FormInput
                type="text"
                name="name"
                placeholder="Company Name"
                value={company.name}
                onChange={handleInputChange}
                required
              />
              <FormInput
                type="text"
                name="nip"
                placeholder="NIP (10 digits)"
                value={company.nip}
                onChange={handleInputChange}
                required
                pattern="\d{10}"
                title="NIP must be exactly 10 digits."
              />
              <FormInput
                type="text"
                name="country"
                placeholder="Country"
                value={company.country}
                onChange={handleInputChange}
                required
              />
              <Button type="button" onClick={nextStep}>
                Next
              </Button>
            </Form>
          </>
        )}

        {currentStep === 2 && (
          <>
            <FormTitle>Step 2: Address Details</FormTitle>
            <Form onSubmit={handleSubmitCompany}>
              <FormInput
                type="text"
                name="city"
                placeholder="City"
                value={company.city}
                onChange={handleInputChange}
                required
              />
              <FormInput
                type="text"
                name="postalCode"
                placeholder="Postal Code"
                value={company.postalCode}
                onChange={handleInputChange}
                required
              />
              <FormInput
                type="text"
                name="street"
                placeholder="Street"
                value={company.street}
                onChange={handleInputChange}
                required
              />
              <div style={{ display: "flex", justifyContent: "space-between" }}>
                <Button
                  type="button"
                  onClick={prevStep}
                  style={{ width: "48%" }}
                >
                  Back
                </Button>
                <Button type="submit" style={{ width: "48%" }}>
                  Submit
                </Button>
              </div>
            </Form>
          </>
        )}

        <LogoutButton onClick={handleLogout}>Logout</LogoutButton>
      </DashboardContent>
    </DashboardWrapper>
  );
};

export default Dashboard;
