import axios from 'axios';

axios.defaults.baseURL = process.env.REACT_APP_API_BASE_URL;
axios.defaults.headers['Content-Type'] = 'application/json';

// Interceptor לבקשות
axios.interceptors.request.use(
  (config) => {
    const token = sessionStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor לתשובות
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    // בדיקת שגיאה משרת
    if (error.response) {
      const status = error.response.status;

      // טיפול ב-401 בצורה שקטה
      if (status === 401) {
        alert("please sign in")
        sessionStorage.removeItem('token'); // הסרת הטוקן אם קיים
        window.location.href = '/login'; // הפניה לדף login
        return Promise.resolve(); // מונע את המשך טיפול השגיאה
      }
    }

    // הדפסת שגיאות אחרות ל-console
    console.error('API Error:', error.response || error.message);

    return Promise.reject(error); // המשך הטיפול בשגיאה
  }
);

const apiUrl =process.env.REACT_APP_API_BASE_URL;
//console.log(process.env.VARIABLE_NAME);

export default {
  getTasks: async () => {
    try {
      const result = await axios.get(`${apiUrl}`);
      return result.data;
    } catch (error) {
      console.error("Error fetching tasks:", error);
      throw error; // אתה יכול לטפל בשגיאה בצד הלקוח אם יש צורך
    }
  },

  addTask: async (name) => {
    const newTask = {
      name: name,
      isComplete: false
    };
    try {
      await axios.post(`${apiUrl}`, newTask);  // Send task data to the server
    } catch (error) {
      console.error("Error adding task:", error);
      throw error; // טיפול בשגיאה
    }
  },

  setCompleted: async (id, isComplete) => {
    try {
      await axios.put(`${apiUrl}${id}`, { isComplete: isComplete });
    } catch (error) {
      console.error("Error updating task:", error);
      throw error; // טיפול בשגיאה
    }
  },

  deleteTask: async (id) => {
    try {
      await axios.delete(`${apiUrl}${id}`);
    } catch (error) {
      console.error("Error deleting task:", error);
      throw error; // טיפול בשגיאה
    }
  }
};
