import { useState } from 'react';

function ImageCarousel({ images, onImageSelect }) {
  const [currentIndex, setCurrentIndex] = useState(0);

  const goToPrevious = () => {
    const newIndex = currentIndex === 0 ? images.length - 1 : currentIndex - 1;
    setCurrentIndex(newIndex);
    onImageSelect(images[newIndex].name);
  };

  const goToNext = () => {
    const newIndex = currentIndex === images.length - 1 ? 0 : currentIndex + 1;
    setCurrentIndex(newIndex);
    onImageSelect(images[newIndex].name);
  };

  return (
    <div style={{ maxWidth: '256px', margin: '20px auto' }}>
      <div style={{ position: 'relative', height: '512px', backgroundColor: '#f0f0f0', borderRadius: '8px', overflow: 'hidden' }}>
        <img
          src={images[currentIndex].url}
          alt={images[currentIndex].name}
          style={{ width: '100%', height: '100%', objectFit: 'cover' }}
        />
       
      </div>
        <button
          onClick={goToPrevious}
          style={{ position: 'absolute', left: '50px', top: '50%', transform: 'translateY(-50%)', background: 'transparent', border: 'none', cursor: 'pointer', fontSize: '48px', color: 'white', textShadow: '0 2px 4px rgba(0,0,0,0.5)', padding: '0' }}
        >
          ‹
        </button>
        
        <button
          onClick={goToNext}
          style={{ position: 'absolute', right: '50px', top: '50%', transform: 'translateY(-50%)', background: 'transparent', border: 'none', cursor: 'pointer', fontSize: '48px', color: 'white', textShadow: '0 2px 4px rgba(0,0,0,0.5)', padding: '0' }}
        >
          ›
        </button>
    </div>
  );
}

export default ImageCarousel;