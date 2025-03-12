<template>
    <div class="login-container">
      <h2>Авторизация</h2>
      <input v-model="login" placeholder="Логин" maxlength="20"/>
      <input v-model="password" type="password" placeholder="Пароль" maxlength="20"/>
      <button @click="handleLogin" :disabled="isLoading">
        {{ isLoading ? "Вход..." : "Войти" }}
      </button>
      <p>Нет аккаунта? <router-link to="/auth/register">Зарегистрироваться</router-link></p>
    </div>
  </template>
  
  <script setup>
    import { ref } from 'vue';
    import { useAuthStore } from '@/stores/authStore';
    import { useRouter } from 'vue-router';
    import { useToast } from 'vue-toastification';
  
    const authStore = useAuthStore();
    const router = useRouter();
  
    const toast = useToast();
    const toastOptions = {
      timeout: 3000,
    };
  
    const login = ref('');
    const password = ref('');
  
    const isLoading = ref(false);
  
  
    const roleRoutes = {
      Admin: "/app/users",
      Manager: "/app/orders",
      Customer: "/app/catalog",
    };
  
    const handleLogin = async () => {
      if (!login.value || !password.value) {
        toast.warning("Введите логин и пароль", toastOptions);
        return;
      }
  
      isLoading.value = true;
  
      const success = await authStore.login(login.value, password.value);
      isLoading.value = false;
  
      if (success) {
        toast.success("Добро пожаловать!", toastOptions);
        router.push(roleRoutes[authStore.role] || "/");
      }
    };
  </script>
  
  <style scoped>
    .login-container {
      width: 400px;
      margin: auto;
      text-align: center;
      padding: 20px;
      border-radius: 15px;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
      background: white;
    }
    input {
      display: block;
      width: 100%;
      margin: 15px 0px;
      padding: 10px;
      border: 1px solid #ccc;
      border-radius: 5px;
      font-size: 14px;
    }
  
    input:focus {
      border-color: #2c3e50;
      outline: none;
    }
  
    button {
      width: 100%;
      padding: 10px;
      background-color: #2c3e50;
      color: white;
      border: none;
      border-radius: 5px;
      cursor: pointer;
      transition: 0.3s;
    }
  
    button:hover {
      background-color: #34495e;
    }
  
    button:disabled {
      background-color: #95a5a6;
      cursor: not-allowed;
    }
  
    p {
      margin-top: 15px 0 0 0;
    }
  
    * {
      box-sizing: border-box;
    }
  </style>
  