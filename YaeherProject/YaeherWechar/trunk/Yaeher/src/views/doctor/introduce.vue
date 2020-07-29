<template>
    <div class="introduce padding-top">
        <mt-header fixed title="简介">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="introduce-case">
                <mt-field  @input = "descInput" label="简介" placeholder="填写简介信息" type="textarea" rows="6" v-model="resume" :attr="{ maxlength: maxReplyLength }"></mt-field>
                <p>可输入{{remark}}字</p>
                <div class="okCase">
                    <mt-button @click="alterUserInfo" type="primary" class="mint-button--large">保存</mt-button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { createSecret } from 'assets/js/common.js'
export default {
    data () {
        return {
            id: 0,
            resume: '',
            maxReplyLength: 300,
            remark: 300
        }
    },
    methods: {
        descInput() {
            let txtVal = this.resume.length
            this.remark = this.maxReplyLength - txtVal
        },
        getUserInfo() { // 请求个人信息
            this.instance.yaeherDoctorD({
            })
                .then((response) => {
                    const userdetail = response.data.result.item
                    console.log(userdetail)
                    this.resume = userdetail.resume
                    if(!this.resume) this.resume = ''
                    console.log(this.resume.length)
                    this.id = userdetail.id
                    const userId = userdetail.id
                    window.sessionStorage.setItem('userId', userId)
                })
                .catch((error) => {
                }) 
        },
        alterUserInfo() { // 修改简介
            const resume = this.resume
            const id = this.id
            this.instance.updateYaeherDoctorResumeD({
                id,
                resume
            })
                .then((response) => {
                    Toast('修改成功')
                    this.$router.push({ path: '/doctor-info-list'})
                })    
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getUserInfo()
    },
    created () {
        this.maxReplyLength = parseInt(window.sessionStorage.getItem('maxReplyLength'))
        this.remark = this.maxReplyLength
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.introduce {
    .introduce-case {
        margin: 10px; 
        padding: 10px 10px 20px; 
        background: $color-wfont;
        .mint-cell-wrapper {
            background-image: none;
        }
        .mint-cell-title {
            display: none;
        }
        .okCase {
            padding-top: 10px;
        }
        p {
            text-align: right;
            color: $color-bfont;
            font-size: $font-m;
            padding-right: 5px;
        }
    }
}


</style>