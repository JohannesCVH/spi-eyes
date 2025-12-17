<template >
    <video ref="streamElement" class="w-100"></video>
</template>

<script>
    import HLS from 'hls.js';

    export default {
        name: 'Stream',
        data() {
            return {
                hls: null
            }
        },
        mounted() {
            const streamElement = this.$refs.streamElement;
            if (!streamElement) return;

            this.hls = new HLS();
            this.hls.loadSource('http://localhost:5002/api/Stream/Stream1')
            this.hls.attachMedia(streamElement);

            this.hls.on(HLS.Events.MANIFEST_PARSED, () => {
                streamElement.play();
            });

            this.hls.on(HLS.Events.ERROR, (err) => {
                console.error('HLS error: ', err);
            });
        },
        beforeUnmount() {
            if (this.hls) this.hls.destroy();
        }
    }
</script>

<style scoped>
    
</style>