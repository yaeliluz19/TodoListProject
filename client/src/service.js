
// // import axios from 'axios';

// // // הגדרת ה-baseURL של ה-API
// // axios.defaults.baseURL = 'http://localhost:5206/';  // עדכן לכתובת ה-API שלך

// // // הגדרת headers ברירת מחדל
// // axios.defaults.headers['Content-Type'] = 'application/json';
// // const apiUrl = "http://localhost:5206/";
// // // הוספת interceptor לתפיסת שגיאות
// // axios.interceptors.response.use(
// //   (response) => response, // אם אין שגיאה מחזירים את התגובה כרגיל
// //   (error) => {
// //     console.error('API Error:', error.response || error.message);
// //     return Promise.reject(error); // מחזירים את השגיאה הלאה
// //   }
// // );

// // export default {
// //   getTasks: async () => {
// //     const result = await axios.get(`${apiUrl}`);
// //     return result.data;
// //   },

// //   addTask: async (name) => {
// //     const newTask = {
// //       name: name,
// //       isComplete: false
// //     };

// //     await axios.post(`${apiUrl}`, newTask);  // Send task data to the server
// //   },

// //   setCompleted: async (id, isComplete) => {
// //     await axios.put(`${apiUrl}${id}`, { isComplete: isComplete });
// //   },

// //   deleteTask: async (id) => {
// //     await axios.delete(`${apiUrl}${id}`);
// //   }
  
// // };
// import axios from 'axios';

// // הגדרת ה-baseURL של ה-API
// axios.defaults.baseURL = 'http://localhost:5206/';  // עדכן לכתובת ה-API שלך

// // הגדרת headers ברירת מחדל
// axios.defaults.headers['Content-Type'] = 'application/json';

// // interceptor לתפיסת שגיאות ב-response
// axios.interceptors.response.use(
//   (response) => response, // אם אין שגיאה, מחזירים את התגובה כרגיל
//   (error) => {
//     console.error('API Error:', error.response || error.message);
//     if (error.response && error.response.status === 401) {
//       // אם השגיאה היא 401, הפנה לדף התחברות
//       window.location.href = '/login'; // לדף התחברות
//     }
//     return Promise.reject(error); // מחזירים את השגיאה הלאה
//   }
// );

// // אם יש JWT ב-localStorage, הוסף אותו ל-header של כל בקשה
// const token = localStorage.getItem('token');
// if (token) {
//   axios.defaults.headers['Authorization'] = `Bearer ${token}`;
// }

// const apiUrl = "http://localhost:5206/";

// export default {
//   getTasks: async () => {
//     try {
//       const result = await axios.get(`${apiUrl}`);
//       return result.data;
//     } catch (error) {
//       console.error("Error fetching tasks:", error);
//       throw error; // אתה יכול לטפל בשגיאה בצד הלקוח אם יש צורך
//     }
//   },

//   addTask: async (name) => {
//     const newTask = {
//       name: name,
//       isComplete: false
//     };

//     try {
//       await axios.post(`${apiUrl}`, newTask);  // Send task data to the server
//     } catch (error) {
//       console.error("Error adding task:", error);
//       throw error; // טיפול בשגיאה
//     }
//   },

//   setCompleted: async (id, isComplete) => {
//     try {
//       await axios.put(`${apiUrl}${id}`, { isComplete: isComplete });
//     } catch (error) {
//       console.error("Error updating task:", error);
//       throw error; // טיפול בשגיאה
//     }
//   },

//   deleteTask: async (id) => {
//     try {
//       await axios.delete(`${apiUrl}${id}`);
//     } catch (error) {
//       console.error("Error deleting task:", error);
//       throw error; // טיפול בשגיאה
//     }
//   }
// };
import axios from 'axios';

// הגדרת ה-baseURL של ה-API
axios.defaults.baseURL = 'http://localhost:5206/';  // עדכן לכתובת ה-API שלך

// הגדרת headers ברירת מחדל
axios.defaults.headers['Content-Type'] = 'application/json';

// interceptor לתפיסת שגיאות ב-response
axios.interceptors.response.use(
  (response) => response, // אם אין שגיאה, מחזירים את התגובה כרגיל
  (error) => {
    console.error('API Error:', error.response || error.message);
    if (error.response && error.response.status === 401) {
      // אם השגיאה היא 401, הפנה לדף התחברות
      window.location.href = '/login'; // לדף התחברות
    }
    return Promise.reject(error); // מחזירים את השגיאה הלאה
  }
);

// אם יש JWT ב-localStorage, הוסף אותו ל-header של כל בקשה
const token = sessionStorage.getItem('token');
if (token) {
  axios.defaults.headers['Authorization'] = `Bearer ${token}`;
}

const apiUrl = "http://localhost:5206/";

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
