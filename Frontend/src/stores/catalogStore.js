import { defineStore } from 'pinia';
import { getFilteredProducts, getCategories, addProduct, updateProduct, deleteProduct } from '@/api/catalog.js';

export const useCatalogStore = defineStore('catalog', {
  actions: {
    async getProducts(params) {
        try {
            const response = await getFilteredProducts(params);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка загрузки продуктов:', error);
        }
    },

    async getCategories() {
        try {
            const response = await getCategories();
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка загрузки категорий:', error);
        }
    },

    async addProduct(newProduct) {
        try {
            const response = await addProduct(newProduct);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка добавления продукта:', error);
        }
    },

    async updateProduct(productId, editProduct) {
        try {
            const response = await updateProduct(productId, editProduct);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка изменения продукта:', error);
        }
    },

    async deleteProduct(id) {
        try {
            const response = await deleteProduct(id);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка удаления продукта:', error);
        }
    },
  }
});
