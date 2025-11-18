// BlazorDiagrams JavaScript interop functions

window.BlazorDiagrams = {
    /**
     * Gets the mouse position relative to an SVG element
     * @param {MouseEvent} event - The mouse event
     * @param {HTMLElement} svgElement - The SVG element
     * @returns {Object} An object with x and y coordinates
     */
    getMousePositionInSvg: function (event, svgElement) {
        const rect = svgElement.getBoundingClientRect();
        return {
            x: event.clientX - rect.left,
            y: event.clientY - rect.top
        };
    },

    /**
     * Gets the bounding rectangle of an SVG element
     * @param {HTMLElement} element - The element to measure
     * @returns {DOMRect} The bounding rectangle
     */
    getSvgElementRect: function (element) {
        return element.getBoundingClientRect();
    },

    /**
     * Exports an SVG to PNG format
     * @param {string} svgContent - The SVG content as a string
     * @param {number} width - The desired width of the PNG
     * @param {number} height - The desired height of the PNG
     * @returns {Promise<string>} A data URL containing the PNG image
     */
    exportToPng: function (svgContent, width, height) {
        return new Promise((resolve, reject) => {
            try {
                // Create a canvas element
                const canvas = document.createElement('canvas');
                canvas.width = width;
                canvas.height = height;
                const ctx = canvas.getContext('2d');

                // Create an image element
                const img = new Image();
                
                // Convert SVG to data URL
                const svgBlob = new Blob([svgContent], { type: 'image/svg+xml;charset=utf-8' });
                const url = URL.createObjectURL(svgBlob);

                img.onload = function () {
                    // Fill white background
                    ctx.fillStyle = 'white';
                    ctx.fillRect(0, 0, width, height);
                    
                    // Draw the SVG image onto the canvas
                    ctx.drawImage(img, 0, 0, width, height);
                    
                    // Convert canvas to PNG data URL
                    const pngDataUrl = canvas.toDataURL('image/png');
                    
                    // Clean up
                    URL.revokeObjectURL(url);
                    
                    resolve(pngDataUrl);
                };

                img.onerror = function (error) {
                    URL.revokeObjectURL(url);
                    reject(error);
                };

                img.src = url;
            } catch (error) {
                reject(error);
            }
        });
    },

    /**
     * Downloads a file from a data URL
     * @param {string} dataUrl - The data URL of the file
     * @param {string} fileName - The name to save the file as
     */
    downloadFile: function (dataUrl, fileName) {
        const link = document.createElement('a');
        link.href = dataUrl;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    },

    /**
     * Gets the bounding client rect of an element
     * @param {HTMLElement} element - The element to measure
     * @returns {DOMRect} The bounding rectangle
     */
    getBoundingClientRect: function (element) {
        return element.getBoundingClientRect();
    },

    /**
     * Copies text to clipboard
     * @param {string} text - The text to copy
     * @returns {Promise<void>}
     */
    copyToClipboard: async function (text) {
        try {
            await navigator.clipboard.writeText(text);
        } catch (err) {
            // Fallback for older browsers
            const textarea = document.createElement('textarea');
            textarea.value = text;
            textarea.style.position = 'fixed';
            textarea.style.opacity = '0';
            document.body.appendChild(textarea);
            textarea.select();
            document.execCommand('copy');
            document.body.removeChild(textarea);
        }
    }
};

