<template>
    <div class="basic-info padding-top">
        <mt-header fixed title="基本资料">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="patient-info add-number-case">
                <mt-field label="名字" readonly placeholder="" v-model="doctorName"></mt-field>
                <div class="illness border-top">
                    <div class="illness-case">
                        <p class="illness-label">性别</p>
                        <p class="illness-value">{{sex1}}</p>
                    </div>
                </div>
                <mt-field label="所在医院"  placeholder="请输入所在医院" v-model="hospitalName"></mt-field>
                <mt-field label="所在科室"   placeholder="请输入所在科室" v-model="department"></mt-field>
                <mt-field label="职称"   placeholder="请输入职称" v-model="title"></mt-field>
                <mt-field label="工作年限" placeholder="工作年限" v-model="workYear"></mt-field>
            </div>
            <div class="okCase numberOk">
                <mt-button @click="alterUserInfo" type="primary" class="mint-button--large">保存</mt-button>
            </div>
            
        </div>
        <mt-picker :slots="slots" ref="picker" :showToolbar="true" v-show="show">
            <div @click="selectSex()" class="slots-no">取消</div>
            <div @click="getPickerValue(true)" class="slots-ok">确认</div>
        </mt-picker>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            doctorName: '',
            hospitalName: '',
            sex: 0,
            sex1: '',
            department: '',
            title: '',
            workYear: '',
            show: false,
            slots: [
                {
                    flex: 1,
                    values: ['男', '女'],
                    className: 'slot1',
                    textAlign: 'center'
                }
            ]
        }
    },
    methods: {
        getPickerValue() {     
            this.show = !this.show
            this.sex1 = this.$refs.picker.getValues()[0]
            const sexArry = ['', '男', '女']
            this.sex = sexArry.indexOf(this.sex1)
        },
        selectSex() {
            this.show = !this.show
            
        },
        getUserInfo() { // 请求个人信息
            this.instance.yaeherDoctorD({
            })
                .then((response) => {
                    const userdetail = response.data.result.item
                    this.doctorName = userdetail.doctorName
                    this.hospitalName = userdetail.hospitalName
                    this.workYear = userdetail.workYear
                    this.department = userdetail.department
                    this.title = userdetail.title
                    this.sex = userdetail.sex
                    const sexStatus = ['', '男', '女']
                    this.sex1 = sexStatus[this.sex]
                    const userId = response.data.result.item.id
                    this.id = userId
                    window.sessionStorage.setItem('userId', userId)
                })
                .catch((error) => {
                }) 
        },
        alterUserInfo() { // 修改个人信息
            const id = this.id
            const doctorName = this.doctorName
            const sex = this.sex
            const hospitalName = this.hospitalName
            const department = this.department
            const title = this.title
            const workYear = this.workYear

            this.instance.updateYaeherDoctorD({
                id,
                doctorName,
                sex,
                hospitalName,
                department,
                title,
                workYear
            })
                .then((response) => {
                    Toast('修改成功')
                    this.$router.push({ 
                        path: '/doctor-info-list'
                    })
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        
    },
	activated () {
		this.getUserInfo()
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.basic-info {
    .add-number-case {margin-top: 10px;}
    .answer {display: flex; font-size: $font-l; padding: 10px 15px; margin: 15px 0; background: $color-wfont;}
    .answer > div {flex: 1;}
    .doctorAn { text-align: right;}
    .answer-name {line-height: 20px;}
    .doctorAn img, .doctorAn span {display: inline-block; vertical-align: middle;}
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
    .upload-row {height: 145px; position: relative; background: $color-wfont; padding: 15px 10px;}
    .uploadImg{font-size: 50px;padding: 20px;border: 2px dashed #aaa;width: 60px; text-align: center; margin: 10px auto;}
    .okCase {padding: 0 10px 20px 10px; background: $color-wfont;}
    .illness {background-color: $color-wfont; padding: 0 0 0 10px;}
    .illness-case {display: flex; padding: 10px 0;}
    .illness-label {width: 105px;}
    .numberOk {padding-top: 15px;}
}

</style>