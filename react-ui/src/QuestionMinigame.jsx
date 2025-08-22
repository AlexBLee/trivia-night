import { useState } from 'react'
import './App.css'

function QuestionMinigame() {
  const [count, setCount] = useState(0)

  return (
    <>
      <h1>Question!</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 3)}>
          count is {count}
        </button>
      </div>
    </>
  )
}

export default QuestionMinigame
