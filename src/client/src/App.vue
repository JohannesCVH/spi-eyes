<template>
  <nav class="navbar navbar-light bg-light mb-4">
      <div class="container-fluid">
        <!-- <router-link class="navbar-brand" to="/">Spi-Eyes</router-link> -->
        <router-link class="navbar-brand" to="/">
          <img src="./assets/logo.png" style="max-width: 48px; box-shadow: 0 4px 8px rgba(0,0,0,0.2); border-radius: 4px;"/>
        </router-link>
        
        <button id="notifBtn">Request Notifications</button>
      </div>
    </nav>
  <div class="container">
    <router-view></router-view>
  </div>
</template>

<script setup lang="ts">
  import { onMounted } from 'vue';

  onMounted(async () => {
    const notifBtn = document.getElementById('notifBtn');
    notifBtn.addEventListener("click", async () => {
      const permission = await Notification.requestPermission();
      if (permission === 'granted') {
        console.log('Notification permission granted.');
        // subscribeUser();
      }
      else {
        console.log('Notification permission not granted.')
      }
    });
  });
</script>

<style scoped></style>