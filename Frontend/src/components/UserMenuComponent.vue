<template>
    <div class="user-menu" ref="menu">
      <button class="user-btn" @click="toggleMenu">
        <User class="icon" /> {{ userName }}
      </button>
      <transition name="fade">
        <div v-if="isMenuOpen" class="dropdown">
          <p class="user-info">Имя: {{ userName }}</p>
          <p class="user-info">Роль: {{ userRole }}</p>
          <button class="logout" @click="logout">Выйти</button>
        </div>
      </transition>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted, onBeforeUnmount } from 'vue';
  import { useAuthStore } from '@/stores/authStore';
  import { User } from 'lucide-vue-next';
  
  
  const authStore = useAuthStore();
  const isMenuOpen = ref(false);
  const userName = ref('');
  const userRole = ref('');
  const menu = ref(null);
  
  // Словарь соответствий ролей
  const roleTranslations = {
    "Admin": "Админ",
    "Manager": "Менеджер",
    "Customer": "Заказчик",
  };
  
  onMounted(() => {
    userName.value = localStorage.getItem('name') || 'Гость';
    const storedRole = localStorage.getItem('role') || 'guest';
    userRole.value = roleTranslations[storedRole] || "Неизвестно";
    document.addEventListener('click', handleClickOutside);
  });
  
  onBeforeUnmount(() => {
    document.removeEventListener('click', handleClickOutside);
  });
  
  const handleClickOutside = (event) => {
    if (menu.value && !menu.value.contains(event.target)) {
      isMenuOpen.value = false;
    }
  };
  
  const toggleMenu = () => {
    isMenuOpen.value = !isMenuOpen.value;
  };
  
  const logout = () => {
    authStore.logout();
    isMenuOpen.value = false;
  };
  </script>
  
  <style scoped>
  .user-menu {
    position: relative;
    display: inline-block;
  }
  
  .user-btn {
    background: linear-gradient(135deg, #4a90e2, #b2cce9);
    color: white;
    border: none;
    cursor: pointer;
    font-size: 18px;
    padding: 10px 14px;
    border-radius: 8px;
    transition: all 0.3s ease;
    box-shadow: 0 4px 8px rgba(0, 122, 255, 0.2);
  }
  
  .icon {
    width: 28px;
    height: 28px;
    vertical-align: middle;
  }
  
  .user-btn:hover {
    background: linear-gradient(135deg, #b2cce9, #2974ca);
    transform: scale(1.05);
  }
  
  .dropdown {
    position: absolute;
    right: 0;
    top: 120%;
    background: white;
    color: black;
    padding: 12px;
    border-radius: 8px;
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
    width: 300px;
    text-align: center;
    opacity: 0;
    transform: translateY(-10px);
    animation: fadeIn 0.3s forwards;
  }
  
  .user-info {
    font-weight: bold;
    margin: 10px 20px 20px 20px;
    color: #333;
  }
  
  .logout {
    background: #e74c3c;
    color: white;
    border: none;
    padding: 8px 12px;
    cursor: pointer;
    border-radius: 6px;
    width: 100%;
    transition: all 0.3s ease;
  }
  
  .logout:hover {
    background: #c0392b;
    transform: scale(1.05);
  }
  
  .fade-enter-active, .fade-leave-active {
    transition: opacity 0.3s, transform 0.3s;
  }
  
  .fade-enter-from, .fade-leave-to {
    opacity: 0;
    transform: translateY(-10px);
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
  