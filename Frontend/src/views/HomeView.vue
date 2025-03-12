<template>
    <div class="auth-layout">
      <main v-if="!authStore.isAuthenticated">
        <section class="welcome">
          <h1>Добро пожаловать!</h1>
          <p>Для продолжения вам нужно авторизоваться.</p>
          <div class="buttons">
            <button @click="goToLogin" class="login-btn">Войти</button>
            <button @click="goToRegister" class="register-btn">Регистрация</button>
          </div>
        </section>
      </main>
      <footer>
        <p class="footer-p">Goods and Orders</p>
      </footer>
    </div>
  </template>
  
  <script setup>
  import { useRouter } from "vue-router";
  import { useAuthStore } from "@/stores/authStore";
  
  const router = useRouter();
  const authStore = useAuthStore();
  
  const goToLogin = () => router.push("/auth/login");
  const goToRegister = () => router.push("/auth/register");
  </script>
  
  <style scoped>
  .auth-layout {
    display: flex;
    flex-direction: column;
    height: 100vh;
    justify-content: center;
    align-items: center;
    text-align: center;
  }
  
  .welcome {
    background: rgba(255, 255, 255, 0.15);
    padding: 30px;
    border-radius: 15px;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
    backdrop-filter: blur(10px);
    width: 400px;
    width: 100%;
    animation: fadeIn 0.5s ease-in-out;
  }
  
  h1 {
    color: black;
    font-size: 26px;
    margin-bottom: 10px;
    text-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
  }
  
  p {
    color: black;
    font-size: 16px;
    margin-bottom: 20px;
  }
  
  .buttons {
    display: flex;
    flex-direction: column;
    gap: 12px;
  }
  
  button {
    padding: 12px;
    font-size: 16px;
    font-weight: bold;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    transition: all 0.3s ease-in-out;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  }
  
  .login-btn {
    color: black;
  }
  
  .login-btn:hover {
    transform: scale(1.05);
    box-shadow: 0 0 15px rgba(0, 114, 255, 0.8);
  }
  
  .register-btn {
    background: #2c3e50;
    color: white;
  }
  
  .register-btn:hover {
    transform: scale(1.05);
    box-shadow: 0 0 15px rgba(255, 117, 140, 0.8);
  }
  
  footer {
    position: absolute;
    bottom: 0;
    width: 100%;
    text-align: center;
    padding: 0px;
    background: #2c3e50;
    color: white;
    font-size: 14px;
  }
  
  .footer-p {
    color: white;
    font-size: 16px;
    margin-bottom: 15px;
  }
  
  * {
    box-sizing: border-box;
  }
  
  @keyframes fadeIn {
    from {
      opacity: 0;
      transform: translateY(-10px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }
  
  @media (max-width: 480px) {
    .welcome {
      padding: 20px;
    }
    h1 {
      font-size: 22px;
    }
    button {
      font-size: 14px;
      padding: 10px;
    }
  }
  </style>
  