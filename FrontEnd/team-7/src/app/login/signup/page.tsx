import Link from 'next/link'
import styles from '../../styles/Login.module.css'
import LoginCard from '../components/LoginCard/loginCard'
import Input from '../components/LoginCard/input/input'
import Button from '../components/LoginCard/button/button'

export default function SignUpPage() {
    return(
        <div className={styles.background}>
        <LoginCard title="Cadastrar sua conta">
        <form className={styles.form}>
                <center><Input type="text" placeholder="Seu nome" /></center>
                <center><Input type="email" placeholder="Seu email" /></center>
                <center><Input type="password" placeholder="Sua senha" /></center>
                <center><Button>Cadastrar</Button></center>
                <Link href="/login">já possui uma conta?</Link> 
                </form>
        </LoginCard>
        </div>
    )
}