import React, { useEffect, useRef, useState } from "react";
import { useAuth } from "src/context/authContext";
import styled from "styled-components";
import trendUp from "src/assets/icons/trend-up.svg";
import Chart from "chart.js/auto";

const DashboardContainer = styled.section`
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  grid-template-rows: repeat(2, 1fr);
  grid-gap: 1.5rem;
  width: 100%;
  padding: 2rem;
  box-sizing: border-box;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    grid-template-rows: repeat(4, auto);
    padding: 1rem;
    width: 100%;
    overflow-x: hidden;
  }
`;

const StandardContainer = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  background-color: ${({ theme }) => theme.white};
  padding: 20px;
  border-radius: 10px;
  grid-column: span 1;
  grid-row: span 1;
  box-sizing: border-box;
  max-width: 100%;

  @media (max-width: 768px) {
    width: 100%;
  }
`;

const StatisticChart = styled.div`
  grid-column: 1 / 2;
  height: 100%;
  background-color: ${({ theme }) => theme.white};
  padding: 20px;
  border-radius: 10px;
  font-size: 1.5rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  max-width: 100%;

  @media (max-width: 768px) {
    grid-column: span 1;
    width: 100%;
  }
`;

const CalendarChart = styled.div`
  grid-column: 2 / 3;
  grid-row: 1 / 3;
  height: 100%;
  background-color: ${({ theme }) => theme.white};
  padding: 20px;
  border-radius: 10px;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  max-width: 100%;

  @media (max-width: 768px) {
    grid-column: span 1;
    width: 100%;
  }
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
  width: 100%;
  height: 300px;
  display: flex;
  justify-content: center;
  align-items: center;

  @media (max-width: 768px) {
    height: 200px;
  }
`;

const WeekView = styled.div`
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  width: 100%;
  gap: 1rem;
  margin-bottom: 1rem;
`;

const Day = styled.div`
  background-color: ${({ theme }) => theme.lightGray};
  padding: 1rem;
  border-radius: 10px;
  text-align: center;

  & > h2 {
    margin-bottom: 1rem;
    font-size: 1.2rem;
  }

  & > div {
    background-color: ${({ theme }) => theme.secondary};
    padding: 0.5rem;
    margin-top: 0.5rem;
    border-radius: 5px;
  }
`;

const EventList = styled.div`
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
`;

const EventItem = styled.div`
  background-color: ${({ theme }) => theme.lightGray};
  padding: 10px;
  border-radius: 5px;
  font-size: 0.9rem;
`;

const Home: React.FC = () => {
  const { user } = useAuth();
  const chartRef = useRef<HTMLCanvasElement | null>(null);
  const chartInstanceRef = useRef<Chart | null>(null);
  const [events, setEvents] = useState<{ [key: string]: string[] }>({
    Monday: ["Event 1", "Event 2"],
    Tuesday: ["Event 3"],
    Wednesday: ["Event 4", "Event 5"],
    Thursday: ["Event 6"],
    Friday: ["Event 7", "Event 8"],
    Saturday: ["Event 9"],
    Sunday: ["Event 10"],
  });

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
          maintainAspectRatio: false,
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
          <span>+11%</span> w porównaniu do poprzedniego miesiąca.
        </TrendParagraph>
      </StandardContainer>

      <StatisticChart>
        <h1>Statystyki dzienne</h1>
        <BarChartContainer>
          <canvas ref={chartRef} />
        </BarChartContainer>
      </StatisticChart>

      <CalendarChart>
        <h1>Kalendarz 2024</h1>
        <WeekView>
          {[
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday",
          ].map((day) => (
            <Day key={day}>
              <h2>{day}</h2>
              <EventList>
                {events[day]?.map((event, index) => (
                  <EventItem key={index}>{event}</EventItem>
                ))}
              </EventList>
            </Day>
          ))}
        </WeekView>
      </CalendarChart>
    </DashboardContainer>
  );
};

export default Home;
