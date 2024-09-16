import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
const Dashboard = () => {
  const [counter, setCounter] = useState(0);
  const token = localStorage.getItem("token");

  const location = useLocation();
  const navigate = useNavigate();
  const userData = location.state?.userData;
  console.log({ userData });

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  useEffect(() => {
    const fetchCounter = async () => {
      try {
        const response = await fetch(
          "https://localhost:7234/api/User/counter",
          {
            method: "GET",
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );

        if (response.ok) {
          const data = await response.json();
          setCounter(data);
        } else {
          console.error("Failed to fetch counter");
        }
      } catch (error) {
        console.error("Error:", error);
      }
    };

    fetchCounter();
  }, []);

  const incrementCounter = async () => {
    try {
      const incrementValue = 1;

      const response = await fetch("https://localhost:7234/api/User/counter", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(incrementValue),
      });

      if (response.ok) {
        setCounter((prevCounter) => prevCounter + incrementValue);
        console.log("Counter updated successfully.");
      } else {
        console.error("Failed to update counter");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const imageUrl = userData.picture || "https://via.placeholder.com/100";

  return (
    <div>
      <h1>Dashboard</h1>
      <h2>
        Welcome, {userData.name} - {userData.email}
      </h2>
      <img
        src={imageUrl}
        alt={userData.name}
        style={{ width: "100px", height: "100px", borderRadius: "50%" }}
        onError={(e) => {
          console.error("Image failed to load:", e.currentTarget.src);
          e.currentTarget.src = "https://via.placeholder.com/100";
        }}
      />
      <p>Counter Value: {counter}</p>

      <button onClick={incrementCounter}>Increment Counter</button>

      <pre>{JSON.stringify(userData, null, 2)}</pre>
      <button onClick={handleLogout}>Logout</button>
    </div>
  );
};

export default Dashboard;
