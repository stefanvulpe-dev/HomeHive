function startCarousel(carouselId) {
    var myCarousel = document.getElementById(carouselId);
    var carousel = new bootstrap.Carousel(myCarousel);
    carousel.cycle();
}

window.createBlob = (data) => {
    const bytes = new Uint8Array(data.data);
    return URL.createObjectURL(new Blob([bytes], { type: data.type }));
}

window.saveAsFile = (filename, content) => {
    const link = document.createElement('a');
    link.href = content;
    link.download = filename;
    link.click();
    URL.revokeObjectURL(content);
}