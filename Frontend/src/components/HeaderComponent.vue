<template>
    <header class="header">
      <nav class="left">
        <router-link v-for="link in filteredLinks" :key="link.to" :to="link.to" active-class="active">
          <component :is="link.icon" class="icon" />
           {{ link.label }}
          <span v-if="link.showCount">({{ cartItemCount }})</span>
        </router-link>
      </nav>
  
      <div class="right">
        <UserMenuComponent/>
      </div>
    </header>
  </template>
  
  <script setup>
    import { computed } from 'vue';
    import { useAuthStore } from '@/stores/authStore';
    import { useCartStore } from '@/stores/cartStore';
    import UserMenuComponent from './UserMenuComponent.vue';
    import { Users, Package, ScrollText, ShoppingCart } from 'lucide-vue-next';
  
    const authStore = useAuthStore();
    const cartStore = useCartStore();
  
    const isAdmin = computed(() => authStore.isAdmin);
    const cartItemCount = computed(() => cartStore.cartItems.length);
  
    // Список всех возможных ссылок
    const navLinks = [
      { to: "/app/users", label: "Пользователи", icon: Users, requiresAdmin: true },
      { to: "/app/catalog", label: "Каталог", icon: Package },
      { to: "/app/orders", label: "Заказы", icon: ScrollText },
      { to: "/app/cart", label: "Корзина", icon: ShoppingCart, showCount: true, requiresAdmin: false }
    ];
  
    // Фильтрация ссылок в зависимости от роли пользователя
    const filteredLinks = computed(() => 
      navLinks.filter(link => link.requiresAdmin === undefined || link.requiresAdmin === isAdmin.value)
    );
  </script>
  
  <style scoped>
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      background: linear-gradient(135deg, #2c3e50, #34495e);
      color: white;
      padding: 12px 20px;
      box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
    }
  
    .icon {
    width: 40px;
    height: 40px;
  }
  
    .left {
      display: flex;
      gap: 12px;
      flex-grow: 1;
    }
  
    .right {
      display: flex;
      align-items: center;
    }
  
    nav a {
      color: white;
      text-decoration: none;
      padding: 8px 15px;
      border-radius: 8px;
      transition: background 0.3s, transform 0.2s;
      font-size: 30px;
      font-weight: 500;
      display: flex;
      align-items: center;
      gap: 10px;
    }
  
    nav a:hover {
      background: rgba(255, 255, 255, 0.2);
      transform: translateY(-2px);
    }
  
    .router-link-active {
      background: #1abc9c;
      color: white;
      font-weight: 600;
      box-shadow: 0 0 8px rgba(26, 188, 156, 0.6);
    }
  
    .user-info {
      display: flex;
      align-items: center;
      gap: 12px;
      background: rgba(255, 255, 255, 0.15);
      padding: 8px 15px;
      border-radius: 8px;
    }
  
    .user-name {
      font-weight: 600;
      font-size: 14px;
    }
  
    .user-role {
      font-size: 12px;
      opacity: 0.8;
    }
  
    .logout {
      background: #e74c3c;
      color: white;
      border: none;
      padding: 8px 12px;
      cursor: pointer;
      border-radius: 6px;
      transition: background 0.3s, transform 0.2s;
      font-size: 14px;
    }
  
    .logout:hover {
      background: #c0392b;
      transform: scale(1.05);
    }
  </style>