<template>
    <div class="app-layout">
      <AppHeader />
  
      <main>
        <router-view />
      </main>
  
      <AppFooter />
    </div>
  </template>
  
  <script setup>
    import AppHeader from '@/components/HeaderComponent.vue';
    import AppFooter from '@/components/FooterComponent.vue';
  </script>
  
  <style scoped>
    .app-layout {
      display: flex;
      flex-direction: column;
      height: 100vh;
      padding: 0;
    }
  
    main {
      flex-grow: 1;
      display: flex;
      margin: 10px;
      flex-direction: column;
    }
  
    * {
      box-sizing: border-box;
    }
  </style>