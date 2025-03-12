<template>
    <div class="not-found">
      <h1>Ошибка 404</h1>
      <p>Страница не найдена</p>
      <button @click="goHome">На главную</button>
    </div>
  </template>
    
  <script setup>
    import { computed } from 'vue';
    import { useAuthStore } from '@/stores/authStore';
    import { useRouter } from 'vue-router';
    
    const authStore = useAuthStore();
    const router = useRouter();
  
    const isAuthenticated = computed(() => !!authStore.token);
      
    // Перенаправления по ролям
    const roleRedirects = {
      Admin: '/app/users',
      Manager: '/app/orders',
      Customer: '/app/products'
    };
  
    const goHome = () => {
      router.push(isAuthenticated.value ? roleRedirects[authStore.role] || '/' : '/guest');
    };
  
  </script>
    
  <style scoped>
    .not-found {
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      height: 100vh;
      text-align: center;
      background: #f8f9fa;
      color: #2c3e50;
      animation: fadeIn 0.5s ease-in-out;
    }
  
    h1 {
      font-size: 3rem;
      font-weight: bold;
      margin-bottom: 0.5rem;
      text-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1);
    }
  
    p {
      font-size: 1.2rem;
      color: #7a7a7a;
      margin-bottom: 1.5rem;
    }
  
    button {
      padding: 12px 24px;
      font-size: 1rem;
      font-weight: bold;
      background: #3498db;
      color: white;
      border: none;
      border-radius: 8px;
      cursor: pointer;
      transition: all 0.3s ease-in-out;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }
  
    button:hover {
      background: #2980b9;
      transform: scale(1.05);
      box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
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
  
  </style>
    