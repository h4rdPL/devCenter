import React, { useState } from "react";
import { useAuth } from "../../context/authContext";

const Home: React.FC = () => {
  const { user } = useAuth();

  return (
    <>
      <div>Home - {user?.company?.name}</div>
    </>
  );
};

export default Home;
