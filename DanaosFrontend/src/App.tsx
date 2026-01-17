import { useState } from 'react';
import axios from 'axios';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Bar } from 'react-chartjs-2';
import './App.css';

// Register Chart.js components
ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

// Define Types for strict typing 
interface StudentGrade {
  studentName: string;
  averageGrade: number;
}

interface CourseData {
  courseName: string;
  averageGrade: number;
}

function App() {
  
  // State Hooks
  const [students, setStudents] = useState<StudentGrade[]>([]);
  const [chartData, setChartData] = useState<CourseData[]>([]);
  const [loading, setLoading] = useState(false);

  // Function to load all data (Grid + Chart)
  const loadData = async () => {
    setLoading(true);
    try {
      // Fetch Grid Data
      const studentRes = await axios.get('http://localhost:5150/api/Students');
      setStudents(studentRes.data);

      // Fetch Chart Data
      const courseRes = await axios.get('http://localhost:5150/api/Students/courses');
      setChartData(courseRes.data);
    } catch (error) {
      console.error("Error loading data:", error);
      alert("Failed to connect to the backend service.");
    } finally {
      setLoading(false);
    }
  };

  // Function to handle Excel download
  const downloadExcel = () => {
    window.open('http://localhost:5150/api/Students/export', '_blank');
  };

  // Chart Configuration
  const chartOptions = {
    responsive: true,
    plugins: {
      legend: { position: 'top' as const },
      title: { display: true, text: 'Average Grade Per Course' },
    },
    scales: { y: { beginAtZero: true, max: 100 } }
  };

  const chartVisuals = {
    labels: chartData.map(c => c.courseName),
    datasets: [
      {
        label: 'Course Average',
        data: chartData.map(c => c.averageGrade),
        backgroundColor: 'rgba(54, 162, 235, 0.6)',
      },
    ],
  };

  return (
    <div className="container" style={{ maxWidth: '800px', margin: '0 auto', padding: '20px' }}>
      <h1>Student Performance Dashboard</h1>
      
      {/* Control Panel */}
      <div style={{ marginBottom: '20px' }}>
        <button onClick={loadData} disabled={loading} style={{ marginRight: '10px', padding: '10px 20px' }}>
          {loading ? 'Loading...' : 'Load Data'}
        </button>
        
        {students.length > 0 && (
          <button onClick={downloadExcel} style={{ padding: '10px 20px', backgroundColor: '#28a745', color: 'white' }}>
            Export to Excel
          </button>
        )}
      </div>

      {/* Data Grid View */}
      {students.length > 0 && (
        <div style={{ marginBottom: '40px' }}>
          <h3>Student Averages</h3>
          <table border={1} cellPadding={10} style={{ width: '100%', borderCollapse: 'collapse' }}>
            <thead style={{ backgroundColor: '#f2f2f2' }}>
              <tr>
                <th>Student Name</th>
                <th>Average Grade</th>
              </tr>
            </thead>
            <tbody>
              {students.map((s, idx) => (
                <tr key={idx}>
                  <td>{s.studentName}</td>
                  <td>{s.averageGrade.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Chart */}
      {chartData.length > 0 && (
        <div style={{ border: '1px solid #ddd', padding: '20px', borderRadius: '8px' }}>
          <Bar options={chartOptions} data={chartVisuals} />
        </div>
      )}
    </div>
  );
}

export default App;