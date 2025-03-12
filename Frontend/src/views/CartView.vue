<template>
    <div class="cart-container">
      <h1 class="cart-title">
        <ShoppingCart class="icon-title" /> Ваша корзина
      </h1>
      
      <table v-if="paginatedItems.length > 0" class="cart-table">
        <thead>
          <tr>
            <th>Название</th>
            <th>Цена</th>
            <th>Кол-во</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in paginatedItems" :key="item.id">
            <td>{{ item.name }}</td>
            <td>{{ item.price }} ₽</td>
            <td>
              <button class="qty-btn" @click="cartStore.decreaseQuantity(item.id)">
                <Minus class="icon" />
              </button>
              {{ item.quantity }}
              <button class="qty-btn" @click="cartStore.increaseQuantity(item.id)">
                <Plus class="icon" />
              </button>
            </td>
            <td>
              <button class="delete-btn" @click="deleteItem(item.id)">
                <Trash2 class="icon" />
              </button>
            </td>
          </tr>
        </tbody>
      </table>
  
      <p v-else class="empty-cart">Сперва добавьте товары</p>
  
      <div v-if="cartStore.cartItems.length > 0" class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">
          <ArrowLeft class="icon" />
        </button>
        <span>Страница {{ currentPage }} из {{ totalPages }}</span>
        <button @click="nextPage" :disabled="currentPage === totalPages">
          <ArrowRight class="icon" />
        </button>
      </div>
  
      <button @click="placeOrder" v-if="cartStore.cartItems.length > 0" class="order-btn">
        Оформить заказ
      </button>
    </div>
  </template>
  
  <script setup>
  import { ref, computed } from 'vue';
  import { useAuthStore } from '@/stores/authStore';
  import { useCartStore } from '@/stores/cartStore';
  import { useOrderStore } from '@/stores/orderStore';
  import { Minus, Plus, Trash2, ShoppingCart, ArrowLeft, ArrowRight } from 'lucide-vue-next';
  
  import { useToast } from 'vue-toastification';
  
  const toast = useToast();
  const toastOptions = {
    timeout: 3000,
  };
  
  const authStore = useAuthStore();
  const cartStore = useCartStore();
  const orderStore = useOrderStore();
  
  const pageSize = 5;
  const currentPage = ref(1);
  
  const totalPages = computed(() => Math.ceil(cartStore.cartItems.length / pageSize));
  
  const paginatedItems = computed(() => {
    const start = (currentPage.value - 1) * pageSize;
    return cartStore.cartItems.slice(start, start + pageSize);
  });
  
  const deleteItem = (id) => {
    cartStore.removeFromCart(id);
    if (paginatedItems.value.length === 0)
      currentPage.value = 1;
  }
  
  const nextPage = () => {
    if (currentPage.value < totalPages.value) currentPage.value++;
  };
  
  const prevPage = () => {
    if (currentPage.value > 1) currentPage.value--;
  };
  
  const placeOrder = async () => {
    if (!authStore.userId) {
      alert("Ошибка: пользователь не найден!");
      return;
    }
  
    try {
      const orderData = {
        customerId: authStore.userId,
        items: cartStore.cartItems.map(item => ({
          productId: item.id,
          quantity: item.quantity
        }))
      };
  
      await orderStore.createOrder(orderData);
      cartStore.clearCart();
      toast.success("Заказ успешно оформлен", toastOptions);
    } catch {
      
    }
  };
  </script>
  
  <style scoped>
  .cart-container {
    max-width: 90%;
    margin: auto;
    padding: 20px;
    background: #f8f9fa;
    border-radius: 8px;
    box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
  }
  
  .cart-title {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 10px; 
    text-align: center;
    font-size: 24px;
  }
  
  .cart-table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 10px;
  }
  
  .cart-table th, .cart-table td {
    padding: 10px;
    border-bottom: 1px solid #ddd;
    text-align: center;
  }
  
  .cart-table th {
    background: #007bff;
    color: white;
  }
  
  .cart-table tbody {
    display: block;
    min-height: 290px;
  }
  
  .cart-table thead, .cart-table tbody tr {
    display: table;
    width: 100%;
    table-layout: fixed;
  }
  
  .cart-table tbody tr:nth-child(n+1):nth-child(-n+5) {
    height: 40px;
  }
  
  .cart-table tbody tr:empty {
    visibility: hidden;
  }
  
  
  .qty-btn {
    background: none;
    border: 1px solid #000000;
    padding: 5px;
    cursor: pointer;
    margin: 0 5px;
    border-radius: 4px;
    color: rgb(0, 0, 0);
  }
  
  .qty-btn:hover {
    background: #ddd;
  }
  
  .delete-btn {
    background: #dc3545;
    color: white;
    border: none;
    padding: 5px 10px;
    cursor: pointer;
    border-radius: 4px;
  }
  
  .delete-btn:hover {
    background: #c82333;
  }
  
  .empty-cart {
    text-align: center;
    font-size: 18px;
    margin-top: 10px;
  }
  
  .pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 10px;
    margin-top: 15px;
  }
  
  .pagination button {
    background: #007bff;
    color: white;
    border: none;
    padding: 5px 10px;
    cursor: pointer;
    border-radius: 4px;
  }
  
  .pagination button:disabled {
    background: #ccc;
    cursor: not-allowed;
  }
  
  .order-btn {
    width: 100%;
    background: #28a745;
    color: white;
    border: none;
    padding: 10px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 16px;
    margin-top: 15px;
  }
  
  .order-btn:hover {
    background: #218838;
  }
  
  .icon {
    width: 24px;
    height: 24px;
    vertical-align: middle;
  }
  
  .pagination-icon {
    background: yel;
  }
  
  .icon-title {
    width: 30px;
    height: 30px;
  }
  </style>
  