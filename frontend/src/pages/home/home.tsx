import React, { useEffect, useRef } from "react";
import { useAuth } from "src/context/authContext";
import styled from "styled-components";
import trendUp from "src/assets/icons/trend-up.svg";
import Chart from "chart.js/auto";

const DashboardContainer = styled.section`
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  grid-gap: 1.5rem;
  width: 100%;
  padding: 5rem 3rem;

  @media (max-width: 768px) {
    grid-template-columns: 1fr; /* Switch to single column on mobile */
    padding: 3rem 1rem; /* Reduce padding on mobile */
  }

  div {
    background-color: ${({ theme }) => theme.white};
    color: ${({ theme }) => theme.black};
    padding: 20px;
    border-radius: 10px;
    font-size: 1.5rem;

    @media (max-width: 768px) {
      font-size: 1.2rem; /* Adjust font size for mobile */
    }
  }
`;

const StandardContainer = styled.div`
  grid-template-columns: repeat(2, 1fr);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
`;

const StatisticChart = styled.div`
  grid-row: span 2;
  height: 100%;
  background-color: ${({ theme }) => theme.white};
  padding: 20px;
  border-radius: 10px;
  font-size: 1.5rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
`;

const Header = styled.h1`
  font-size: 2.5rem;
  font-weight: bold;

  @media (max-width: 768px) {
    font-size: 2rem;
  }
`;

const TrendParagraph = styled.span`
  display: flex;
  align-items: center;
  img {
    width: 30px;
  }
  span {
    color: green;
    margin-left: 1rem;
  }
`;

const BarChartContainer = styled.div`
  height: 300px;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;

  @media (max-width: 768px) {
    height: 200px;
  }
`;

const Home: React.FC = () => {
  const { user } = useAuth();
  const chartRef = useRef<HTMLCanvasElement | null>(null);
  const chartInstanceRef = useRef<Chart | null>(null);

  useEffect(() => {
    const ctx = chartRef.current?.getContext("2d");
    if (ctx) {
      if (chartInstanceRef.current) {
        chartInstanceRef.current.destroy();
      }

      chartInstanceRef.current = new Chart(ctx, {
        type: "bar",
        data: {
          labels: ["January", "February", "March", "April", "May"],
          datasets: [
            {
              label: "Monthly Revenue",
              data: [1200, 1900, 3000, 500, 2400],
              backgroundColor: "rgba(75, 192, 192, 0.2)",
              borderColor: "rgba(75, 192, 192, 1)",
              borderWidth: 1,
            },
          ],
        },
        options: {
          responsive: true,
          scales: {
            y: {
              beginAtZero: true,
            },
          },
        },
      });
    }

    return () => {
      if (chartInstanceRef.current) {
        chartInstanceRef.current.destroy();
      }
    };
  }, []);

  return (
    <DashboardContainer>
      <StandardContainer>
        <p>Przychód w ostatnim miesiącu</p>
        <Header>214,402 PLN</Header>
        <TrendParagraph>
          <img src={trendUp} alt="trend_icon" />
          <span>+11% </span> w porównaniu do poprzedniego msc.
        </TrendParagraph>
      </StandardContainer>
      <div>Home - {user?.company?.name}</div>

      <StatisticChart>
        <h1>Statystyki dzienne</h1>
        <BarChartContainer>
          <canvas ref={chartRef} />
        </BarChartContainer>
      </StatisticChart>
      <div>
        <p>Aktywni klienci</p>
        <h1>6412</h1>
        <span>+4% w porównaniu do poprzedniego msc.</span>
      </div>

      <div>
        <p>Kalendarz 2024</p>
      </div>
    </DashboardContainer>
  );
};

export default Home;
