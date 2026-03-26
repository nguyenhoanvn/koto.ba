

window.notifSettings = (() => {

    let _ctx = null;
    let _unlocked = false;

    function getCtx() {
        if (!_ctx) _ctx = new (window.AudioContext || window.webkitAudioContext)();
        return _ctx;
    }

    function unlockAudio() {
        if (_unlocked) return;
        const ctx = getCtx();

        // create a silent buffer to unlock audio
        const buffer = ctx.createBuffer(1, 1, 22050);
        const source = ctx.createBufferSource();
        source.buffer = buffer;
        source.connect(ctx.destination);

        source.start(0);

        if (ctx.state === 'suspended') {
            ctx.resume();
        }

        _unlocked = true;
        console.log("🔓 Audio unlocked");
    }

    document.addEventListener('click', unlockAudio, { once: true });
    document.addEventListener('keydown', unlockAudio, { once: true });

    function playSound(volume = 0.7) {
        const ctx = getCtx();

        if (ctx.state === 'suspended') {
            ctx.resume();
        }

        if (!_unlocked) {
            console.warn("🔇 Audio not unlocked yet");
            return;
        }

        const vol = Math.max(0, Math.min(1, volume));

        const masterGain = ctx.createGain();
        masterGain.gain.setValueAtTime(vol, ctx.currentTime);
        masterGain.connect(ctx.destination);

        const notes = [
            { freq: 880, startAt: 0, dur: 0.12 },
            { freq: 1318, startAt: 0.1, dur: 0.18 },
        ];

        notes.forEach(({ freq, startAt, dur }) => {
            const osc = ctx.createOscillator();
            const gain = ctx.createGain();

            osc.type = 'sine';
            osc.frequency.setValueAtTime(freq, ctx.currentTime + startAt);

            gain.gain.setValueAtTime(0, ctx.currentTime + startAt);
            gain.gain.linearRampToValueAtTime(0.8, ctx.currentTime + startAt + 0.02);
            gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + startAt + dur);

            osc.connect(gain);
            gain.connect(masterGain);

            osc.start(ctx.currentTime + startAt);
            osc.stop(ctx.currentTime + startAt + dur + 0.05);
        });
    }


    /**
     * Returns the current notification permission state.
     * @returns {"default"|"granted"|"denied"}
     */
    function getPermission() {
        if (!('Notification' in window)) return 'denied';
        return Notification.permission;
    }

    /**
     * Requests browser notification permission.
     * @returns {Promise<"default"|"granted"|"denied">}
     */
    async function requestPermission() {
        if (!('Notification' in window)) return 'denied';
        const result = await Notification.requestPermission();
        return result;
    }

    /**
     * Shows a browser notification if permission is granted.
     * @param {string} title
     * @param {string} body
     */
    function showNotification(title, body, avatarUrl) {
        if (!('Notification' in window)) return;
        if (Notification.permission !== 'granted') return;

        if (document.visibilityState === 'visible') return;

        new Notification(title, {
            body,
            icon: avatarUrl,   // adjust to your favicon path
            badge: '/favicon.png',
            silent: true,           // we handle sound ourselves
            tag: 'kotoba-message',  // replace so we don't stack
            renotify: true,
        });
    }

    return { playSound, getPermission, requestPermission, showNotification };

})();

// Scroll helper used by chat message list
window.scrollToBottom = function (element) {
    try {
        if (!element) return;
        if (element.scrollHeight === undefined) return;

        element.scrollTo
            ? element.scrollTo({ top: element.scrollHeight, behavior: 'smooth' })
            : (element.scrollTop = element.scrollHeight);
    } catch (e) {
        console.error('scrollToBottom failed', e);
    }
};

