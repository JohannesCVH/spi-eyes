<template>
    <div class="container">
        <div class="row">
            <div class="col-12">
                <video ref="streamRef" class="w-100" controls autoplay muted playsinline webkit-playsinline></video>
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
    import { onMounted, onUnmounted, ref } from 'vue';

    const props = defineProps({
        cameraName: {
            type: String
        }
    });
    
    const streamRef = ref(null);

    onMounted(async () => {
        await startStream();
    });

    onUnmounted(async () => {
        
    });

    const startStream = async () => {
        const rtcPeerConn = new RTCPeerConnection({
            iceServers: []
        });

        rtcPeerConn.ontrack = (event) => {
            if (streamRef.value) { streamRef.value.srcObject = event.streams[0]; };
        };

        rtcPeerConn.addTransceiver('video', { direction: 'recvonly' });
        rtcPeerConn.addTransceiver('audio', { direction: 'recvonly' });

        const offer = await rtcPeerConn.createOffer();
        await rtcPeerConn.setLocalDescription(offer);

        const offerRes = await fetch(`https://192.168.68.110:8889/${props.cameraName}/whep`, {
            method: 'POST',
            body: offer.sdp,
            headers: { 'Content-Type': 'application/sdp' },
            mode: 'cors'
        });

        if (!offerRes.ok) console.error('WHEP request failed.');

        const answerSdp = await offerRes.text();
        await rtcPeerConn.setRemoteDescription(new RTCSessionDescription({
            type: 'answer',
            sdp: answerSdp
        })); 
    };
</script>

<style scoped>
    
</style>