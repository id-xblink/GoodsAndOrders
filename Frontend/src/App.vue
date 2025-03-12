<script setup>
  import { useCartStore } from '@/stores/cartStore';

  const cartStore = useCartStore();
  cartStore.loadCart();

</script>

<template>
  <router-view /> 
</template>

<style>
  html, body {
    margin: 0;
    padding: 0;
  }
</style>