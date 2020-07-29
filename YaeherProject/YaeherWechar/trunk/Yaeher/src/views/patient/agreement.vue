<template>
    <div class="agreement padding-top">
        <router-view/>
        <mt-header fixed title="咨询须知">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="agree">
                <p>1、欢迎使用怡禾健康的付费咨询服务。</p>
                <p>2、使用本服务时，请提供真实准确的信息以帮助医务人员进行更准确判断。</p>
                <p>3、网络咨询有一定局限性，意见仅供参考，不能作为临床诊断及医疗的依据，也不能替代医生与病人面对面的诊疗。</p>
                <p>4、急重症病情，处方药物的使用问题等不适合网络咨询的问题，建议及时去医院就诊。</p>
                <p>5、医生的建议和意见仅针对咨询时的状态，咨询后如病情如发生变化，尤其是加重，请及时就医。</p>
                <p>6、咨询时请围绕一个问题提问，并将问题描述清楚，同时说明自己想得到的帮助。</p>
                <p>7、图文咨询收到回复后36小时内可就原提问相关疑虑追问两次，电话咨询仅有一次通话，不能发起追问。</p>
                <p>8、图片仅用于补充文字不好描述的病情，比如病损图片，或检查、化验结果，仅以文字图片形式的提问医生可能不会作答。</p>
                <p>9、医生收到咨询后会尽快回复，通常不超过4小时（夜间休息时间除外），但可能因工作或其它原因不能及时回复，如超过24小时未回复，费用将在三个工作日内返回您的账号。</p>
                <p>10、所有咨询可能会被质控人员调阅以作咨询质量控制，怡禾会严格保护咨询者隐私，未经允许不会公开任何内容。</p>
                <p>11、若医生给您推荐非怡禾所提供的服务及产品时，请您仔细辨别，若您采用将与怡禾无关。</p>
                <p>12、咨询时如果遇到问题，可查看咨询常见问题Q&A，如问题仍未解决可将问题通过公众号对话框发送到人工客服。</p>
                <p>13、关于怡禾用户协议更多细则，可参见详细版<router-link to="/agreement1">《怡禾健康用户协议》</router-link>，点击“我同意”代表您已阅读并认同《怡禾健康用户协议》。</p>
                <div @click="goRoute" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">我同意</mt-button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            id: 0,
            price: 0,
            serviceType: ''
        }
    },
    methods: {
        goRoute() {
            const id = this.id
            const price = this.price
            const serviceType = this.serviceType
            this.$router.push({ 
                path: '/consultation',
                query: {
                    id,
                    price,
                    serviceType
                }
            })
        }
    },
    created () {
        document.title = '怡禾健康'
        this.id = parseInt(this.$route.query.id)
        this.price = this.$route.query.price
        this.serviceType = this.$route.query.serviceType
        
    },
    watch: {
        '$route.path': function (newVal, oldVal) {
            console.log(newVal)
            console.log(oldVal)
        },
        deep: true
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.agreement {
    height: 100%; 
    background: $color-wfont;
    .agree { 
        padding: 20px 18px; 
        background: $color-wfont; 
        p {
            font-size: $font-l; 
            color: $color-afont; 
            line-height: 1.7;
            padding-bottom: 5px;
        }
        a {
            color: $default-color;
        }
    }
    .okBtn {
        display: block; 
        padding: 10px 0 ;
    }
}

</style>