<template>
    <div class="login-container">
      <h2>Регистрация</h2>
  
      <input v-model="newUser.name" placeholder="Имя" maxlength="30"/>
      <input v-model="newUser.login" placeholder="Логин" maxlength="20"/>
  
      <input v-model="userPassword" type="password" placeholder="Пароль" maxlength="20"/>
      <p v-if="userPassword.length > 0 && userPassword.length < 5" class="error">
        Пароль должен быть не менее 5 символов
      </p>
  
      <input v-model="newUser.address" placeholder="Адрес" maxlength="200"/>
  
      <button @click="handleRegister" :disabled="isLoading">
        {{ isLoading ? "Регистрация..." : "Зарегистрироваться" }}
      </button>
  
      <p>Уже есть аккаунт? <router-link to="/auth/login">Войти</router-link></p>
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
  
    const newUser = ref({
      name: '',
      login: '',
      address: '',
    });
    const userPassword = ref('');
    const isLoading = ref(false);
  
  
    const handleRegister = async () => {
      if (!newUser.value.name || !newUser.value.login || !newUser.value.address || !userPassword.value) {
        toast.warning("Заполните все поля", toastOptions);
        return;
      }
  
      if (userPassword.value.length < 5) {
        toast.warning("Пароль должен быть не менее 5 символов", toastOptions);
        return;
      }
  
      isLoading.value = true;
  
      try {
        await authStore.registration(newUser.value, userPassword.value);
        
        router.push({
          Admin: "/app/users",
          Manager: "/app/orders",
          Customer: "/app/catalog"
        }[authStore.role] || "/");
      } catch {
        
      } finally {
        isLoading.value = false;
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
  
  .error {
    color: red;
    font-size: 12px;
  }
  
  * {
    box-sizing: border-box;
  }
  </style>