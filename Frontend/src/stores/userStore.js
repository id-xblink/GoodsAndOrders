import { defineStore } from 'pinia';
import { getFilteredUsers, getRoles, addUser, updateUser, deleteUser } from '@/api/user.js';

export const useUserStore = defineStore('user', {
  actions: {
    async getUsers(params) {
        try {
            const response = await getFilteredUsers(params);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка загрузки пользователей:', error);
        }
    },

    async getRoles() {
        try {
            const response = await getRoles();
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка загрузки ролей:', error);
        }
    },

    async addUser(newUser, password) {
        try {
            const response = await addUser(newUser, password);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка добавления пользователя:', error);
        }
    },

    async updateUser(userId, editUser) {
        try {
            const response = await updateUser(userId, editUser);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка изменения пользователя:', error);
        }
    },

    async deleteUser(id) {
        try {
            const response = await deleteUser(id);
            return response;
        } catch (error) {
            throw error;
            console.error('Ошибка удаления пользователя:', error);
        }
    },
  }
});
