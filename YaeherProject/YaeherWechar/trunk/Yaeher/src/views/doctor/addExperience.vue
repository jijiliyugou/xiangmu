<template>
    <div class="add-experience padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.push({path: '/experience'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content add-box">
            <div class="add-case">
                <p class="disease-title">执业经历</p>
                <mt-field label="医院" placeholder="请输入医院" v-model="hospitalName"></mt-field>
                <mt-field label="科室" placeholder="请输入科室" v-model="department"></mt-field>
            </div>
            <div class="okCase">
                <mt-button v-if="type != 'detail'" @click="createlable" type="primary" class="mint-button--large">添加</mt-button>
            </div>
            <div class="okCase">
                <mt-button v-if="type === 'detail'" @click="alterlable" type="primary" class="mint-button--large">保存</mt-button>
            </div>
            <div @click="deleteLable" class="okCase">
                <mt-button v-if="type === 'detail'" type="default" class="mint-button--large">删除</mt-button>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { verifyPrice } from 'assets/js/common.js'
export default {
    data () {
        return {
            id: 0,
            hospitalName: '',
            department: '',
            title: '添加执业经历'
        }
    },
    methods: {
        createlable() { // 添加病种
            const hospitalName = this.hospitalName
            const department = this.department
            // 校验数组
            let verifyJson = [
                {
                    value: hospitalName,
                    msg: '医院不能为空'
                },
                {
                    value: department,
                    msg: '科室不能为空'
                },
            ]

            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return1
            this.instance.createDoctorEmploymentD({
                hospitalName,
                department
            })
                .then((response) => {
                    Toast('添加成功')
                    this.$router.push({ 
                        path: '/experience'
                    })
                })
                .catch((error) => {
                }) 
        },
        experienceDetail() { // 执业经历详情
            const id =  this.id
            this.instance.doctorEmploymentByIdD({
                id
            })
                .then((response) => {
                    const experienceDetail = response.data.result.item
                    this.hospitalName = experienceDetail.hospitalName
                    this.department = experienceDetail.department
                })
                .catch((error) => {
                }) 
        },
        alterlable() { // 修改执业经历
            const id = this.id
            const hospitalName = this.hospitalName
            const department = this.department
            // 校验数组
            let verifyJson = [
                {
                    value: hospitalName,
                    msg: '医院不能为空'
                },
                {
                    value: department,
                    msg: '科室不能为空'
                },
            ]

            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return1
            this.instance.updateDoctorEmploymentD({
                id,
                hospitalName,
                department
            })
                .then((response) => {
                    Toast('修改成功')
                    this.$router.push({ 
                        path: '/experience'
                    })
                })
                .catch((error) => {
                }) 
        },
        deleteLable() { // 删除执业详情
            const id = this.id
            this.instance.deleteDoctorEmploymentD({
                id
            })
                .then((response) => {
                    Toast('删除成功')
                    this.$router.push({ 
                        path: '/experience'
                    })
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        if (this.type === 'detail') {
            this.title = '执业经历详情'
            this.experienceDetail()
        }
        
    },
    created () {
        this.type = this.$route.query.type
        this.id = parseInt(this.$route.query.id)
    }
}
</script>


<style lang="scss">
@import "~assets/sass/base.scss";

.add-experience {
    .add-box {
        .add-case {
            margin: 10px; 
            padding: 10px 15px; 
            background: $color-wfont;
            p {line-height: 30px;}
        }
        .okCase {
            margin: 20px 10px;
            padding: 0;
        }
    }
}


</style>