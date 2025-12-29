<template>
    <div class="container">
        <div class="row">
            <div class="col-12">
                <video ref="streamRef" controls class="w-100"></video>
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
    import HLS from 'hls.js';
    import { onMounted, onUnmounted, ref } from 'vue';

    const props = defineProps({
        cameraName: {
            type: String
        }
    });
    
    const streamRef = ref(null);
    let hls: HLS = null;

    onMounted(async () => {
        if (HLS.isSupported()){console.log('HLS is supported.')}

        hls = new HLS({
            xhrSetup: function (xhr, url) {
                if (url.endsWith('.ts')) {
                    const segmentFileName = url.split('/').pop();
                    const newUrl = `http://localhost:5002/api/Stream/${props.cameraName}/${segmentFileName}`;
                    xhr.open('GET', newUrl, true);
                }
            }
        });
        
        hls.loadSource(`http://localhost:5002/api/Stream/${props.cameraName}`)
        hls.attachMedia(streamRef.value);

        hls.on(HLS.Events.MEDIA_ATTACHED, () => {
            console.log("HLS: Video element attached");
        });

        hls.on(HLS.Events.MANIFEST_PARSED, () => {
            streamRef.value.play();
        });

        hls.on(HLS.Events.ERROR, (err) => {
            console.error('HLS error: ', err);
        });
    });

    onUnmounted(async () => {
        if (hls) {
            console.log('Destroying HLS instance..');
            hls.detachMedia();
            hls.destroy();
            hls = null;
        }
    });
</script>

<style scoped>
    
</style>