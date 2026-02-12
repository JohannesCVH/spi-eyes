<template>
    <div class="toast-container position-fixed top-0 end-0 p-4 opacity-100" style="top: 80px !important;">
        <div ref="toastElem" class="toast" data-bs-autohide="true" data-bs-delay="2000">
            <div class="toast-header">
                <div :class="[ 'toast-type', toastType == ToastType.Sucess ? 'toast-success' : '', toastType == ToastType.Error ? 'toast-error' : '', 'me-2' ]"></div>
                <strong class="me-auto">{{ titleRef }}</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                {{ messageRef }}
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
    import { ref, onMounted } from 'vue';
    import { Toast } from 'bootstrap'; 
    import { ToastType } from '../models/ToastTypes';

    const toastElem = ref(null);
    let toastInstance = null;
    const toastType = ref<ToastType>(null);

    const titleRef = ref('');
    const messageRef = ref('');
    
    onMounted(() => {
        toastInstance = new Toast(toastElem.value);
    });

    const show = (title: string, message: string, type: ToastType) => {
        titleRef.value = title;
        messageRef.value = message;
        toastType.value = type;
        toastInstance.show();
    };

    defineExpose({ show });
</script>

<style scoped>
    .toast-header {
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
    }
    .toast-body {
        border-bottom-left-radius: 6px;
        border-bottom-right-radius: 6px;
    }
    .toast-type {
        width: 20px;
        height: 20px;
        border-radius: 15px;
    }
    .toast-success {
        background-color: #0084a8;
    }
    .toast-error {
        background-color: crimson;
    }
    .toast-body {
        background-color: white;
    }
</style>