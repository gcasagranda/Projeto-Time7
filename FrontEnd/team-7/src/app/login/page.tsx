import Link from 'next/link'
import styles from '../styles/Login.module.css'
import LoginCard from './components/LoginCard/loginCard'
import Input from './components/LoginCard/input/input'
import Button from './components/LoginCard/button/button'

export default function LoginPage() {
    return(
        <div className={styles.background}>
            <LoginCard title="Entre em sua conta">
                <form className={styles.form}>
                <center><Input type="email" placeholder="login" /></center>
                <center><Input type="password" placeholder="senha" /></center>
                <center><Button>Acessar</Button></center> 
                <Link href="/login/signup">Ainda não possui conta?</Link> 
                </form>
            </LoginCard>
        </div>
    )
}