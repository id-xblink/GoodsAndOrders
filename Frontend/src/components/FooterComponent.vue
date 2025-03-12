<template>
    <footer class="app-footer">
      <div class="status">
        <span>{{ statusText }}</span>
        <Circle class="icon " :class="statusClass" />
        <span v-if="currentSection"> | Раздел: {{ currentSection }}</span>
      </div>
    </footer>
  </template>
  
  <script setup>
    import { computed } from 'vue';
    import { useRoute } from 'vue-router';
    import { useSystemStore } from '@/stores/systemStore.js';
    import { Circle } from 'lucide-vue-next';
  
    const route = useRoute();
    const systemStore = useSystemStore();
  
    // Карта разделов
    const sectionsMap = {
      '/app/catalog': 'Каталог',
      '/app/orders': 'Заказы',
      '/app/cart': 'Корзина',
      '/app/users': 'Пользователи'
    };

    // Определение текущего раздела
    const currentSection = computed(() => sectionsMap[route.path]?.toString() || null);
    const apiStatus = computed(() => systemStore.apiStatus);
    
  
    const statusClass = computed(() => {
      if (apiStatus.value === 'online') return 'success';
      if (apiStatus.value === 'offline') return 'error';
      return 'warning';
    });
  
    const statusText = computed(() => {
      if (apiStatus.value === 'online') return 'Сервер доступен';
      if (apiStatus.value === 'offline') return 'Сервер недоступен';
      return 'Статус неизвестен';
    });
  
  </script>
    
  <style scoped>
    .app-footer {
      position: fixed;
      bottom: 0;
      width: 100%;
      background: #2c3e50;
      color: white;
      text-align: center;
      font-size: 16px;
      display: flex;
      justify-content: center;
      align-items: center;
      padding: 12px 20px;
      box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
    }
  
  .icon {
    width: 16px;
    height: 16px;
  }
  
  .status {
    display: flex;
    align-items: center;
    gap: 5px;
  }
  
  .success { 
    color: lime; 
    fill: lime;
  }
  .error { 
    color: red; 
    fill: red;
  }
  .warning { 
    color: yellow; 
    fill: yellow;
  }
  </style>
    