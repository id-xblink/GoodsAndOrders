import { defineStore } from 'pinia';
import { useAuthStore } from '@/stores/authStore';

export const useCartStore = defineStore('cart', {
  state: () => {
    const authStore = useAuthStore();
    return {
      cartItems: JSON.parse(localStorage.getItem(`cart_${authStore.userId}`)) || [],
      categories: [],
      authStore,
    };
  },

  actions: {
    loadCart() {
      const userCart = localStorage.getItem(`cart_${this.authStore.userId}`);
      this.cartItems = userCart ? JSON.parse(userCart) : [];
    },

    increaseQuantity(id) {
      const item = this.cartItems.find(item => item.id === id);
      if (item && item.quantity < 1000) {
        item.quantity++;
        this.saveCart();
      }
    },

    decreaseQuantity(id) {
      const item = this.cartItems.find(item => item.id === id);
      if (item && item.quantity > 1) {
        item.quantity--;
        this.saveCart();
      }
    },

    addToCart(product, quantity = 1) {
      const item = this.cartItems.find(item => item.id === product.id);
      if (item) {
        if (item.quantity + quantity > 1000)
          throw error;
        item.quantity += quantity
      } else {
        this.cartItems.push({ ...product, quantity });
      }
      this.saveCart();
    },

    removeFromCart(id) {
      this.cartItems = this.cartItems.filter(item => item.id !== id);
      this.saveCart();
    },

    clearCart() {
      this.cartItems = [];
      this.saveCart();
    },

    saveCart() {
      if (this.authStore.userId) {
        localStorage.setItem(`cart_${this.authStore.userId}`, JSON.stringify(this.cartItems));
      }
    },

    clearCartOnLogout() {
      this.cartItems = [];
    }
  }
});