import { defineStore } from 'pinia';
import { getDetailedOrder, getUserOrders, getStatuses, createOrder, updateOrderToStart, updateOrderToFinish, deleteOrder } from '@/api/order.js';

export const useOrderStore = defineStore('order', {
  actions: {
    async getDetailedOrder(orderId) {
        try {
            const response = await getDetailedOrder(orderId);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка загрузки заказа:', error);
        }
    },

    async getUserOrders(isAdmin, userId, params) {
        try {
            const response = await getUserOrders(isAdmin, userId, params);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка загрузки заказов:', error);
        }
    },

    async getStatuses() {
        try {
            const response = await getStatuses();
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка загрузки статусов:', error);
        }
    },

    //cartview
    async createOrder(orderData) {
        try {
            const response = await createOrder(orderData);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка добавления заказа:', error);
        }
    },

    async updateOrderToStart(orderStart) {
        try {
            const response = await updateOrderToStart(orderStart);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка изменения заказа:', error);
        }
    },

    async updateOrderToFinish(orderFinish) {
        try {
            const response = await updateOrderToFinish(orderFinish);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка изменения заказа:', error);
        }
    },
    

    async deleteOrder(orderId) {
        try {
            const response = await deleteOrder(orderId);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка удаления заказа:', error);
        }
    },
  }
});
