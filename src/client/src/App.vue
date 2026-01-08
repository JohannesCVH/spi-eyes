<template>
  <nav class="navbar navbar-light bg-light mb-4">
      <div class="container-fluid">
        <!-- <router-link class="navbar-brand" to="/">Spi-Eyes</router-link> -->
        <router-link class="navbar-brand" to="/">
          <img src="./assets/logo.png" style="max-width: 48px; box-shadow: 0 4px 8px rgba(0,0,0,0.2); border-radius: 4px;"/>
        </router-link>
      </div>
    </nav>
  <div class="container">
    <router-view></router-view>
  </div>
</template>

<script setup lang="ts">
  
</script>

<style scoped></style>