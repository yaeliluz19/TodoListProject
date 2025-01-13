
import axios from 'axios';
/*olddddddddddddddddddddddddddddddddddd */
/* 
axios.defaults.baseURL = 'https://localhost:44337/';  // עדכן לכתובת ה-API שלך
axios.defaults.headers['Content-Type'] = 'application/json';
axios.interceptors.response.use(
  (response) => response, 
  (error) => {
    console.error('API Error:', error.response || error.message);
    if (error.response && error.response.status === 401) {
      window.location.href = '/login'; // לדף התחברות
    }
    return Promise.reject(error); 
  }
);
const token = sessionStorage.getItem('token');
if (token) {
  axios.defaults.headers['Authorization'] = `Bearer ${token}`;
}
*/
axios.defaults.baseURL = 'https://localhost:44337/';
axios.defaults.headers['Content-Type'] = 'application/json';

axios.interceptors.request.use((config) => {

  const token = sessionStorage.getItem('token');
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error.response || error.message);
    if (error.response && error.response.status === 401) {
      sessionStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);



const apiUrl = "https://localhost:44337/";

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
