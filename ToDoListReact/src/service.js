import axios from 'axios';


axios.defaults.baseURL = "http://localhost:5035";
// axios.defaults.baseURL = "https://localhost:7031";
axios.interceptors.response.use(
  response => response,
  error => {
    // בדיקה האם יש שגיאת תגובה מהשרת
    if (error.response) {
      // הדפסת השגיאה ללוג
      console.error('Response Error:', error.response.status, error.response.data);
    } else if (error.request) {
      // בקשה נשלחה אך לא התקבלה תגובה
      console.error('Request Error:', error.request);
    } else {
      // שגיאה בעת עיבוד הבקשה
      console.error('Error:', error.message);
    }
    // החזרת השגיאה למעבדת הבקשות
    return Promise.reject(error);
  }
);
export default {
  getTasks: async () => {
    const result = await axios.get('');  
    return result.data;
  },
  addTask: async(name)=>{
    console.log('addTask', name)
    const body = {
      name,
     isComplete:false
    };
    const response=await axios.post('',body);
    return response.data;
  },
  setCompleted: async(id, isComplete)=>{
    return await axios.put(`/${id}`, { name: " ", isComplete: isComplete });
  },

  deleteTask:async(id)=>{
    console.log('deleteTask');
    return await axios.delete(`/${id}`);
  }
};
