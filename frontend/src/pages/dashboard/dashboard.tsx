import React from "react";
import { useLocation, useNavigate } from "react-router-dom";

const Dashboard = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const userData = location.state?.userData;

  const handleLogout = () => {
    localStorage.removeItem("token");

    navigate("/");
  };

  if (!userData) {
    return <p>No user data available</p>;
  }

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
      <pre>{JSON.stringify(userData, null, 2)}</pre>
      <button onClick={handleLogout}>Logout</button>
    </div>
  );
};

export default Dashboard;
