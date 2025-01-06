// import React from 'react';
// import ReactDOM from 'react-dom';
// import App from './App';

// ReactDOM.render(<App />, document.getElementById('root'));
import React from 'react';
import ReactDOM from 'react-dom/client';  // שים לב לשם החדש
import App from './App';

// יצירת root עם createRoot
const root = ReactDOM.createRoot(document.getElementById('root'));

// רינדור האפליקציה
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
