<template>
    <div class="experience padding-top">
        <mt-header fixed title="执业经历">
            <a @click="$router.push({path: '/doctor-info-list'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/add-experience" class="right-white">添加</router-link>
            </mt-button> 
        </mt-header>
        <div class="content experience-box">
            <ul v-for="(item, index) in experienceList" :key="index" class="user-list experience-list">
                <router-link tag="li" :to="{path: 'add-experience', query: {id: item.id, type: 'detail'}}">
                    <div class="experience-title">经历{{index+1}}</div>
                    <div>医院 
                        <span>{{item.hospitalName}}</span>
                    </div>
                    <div class="border-none">科室 
                        <span>{{item.department}}</span>
                    </div>
                </router-link>
            </ul>
            
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            experienceList: []
        }
    },
    methods: {
        getlablePage() { // 获取执业经历列表
            this.instance.doctorEmploymentPageD({
            })
                .then((response) => {
                    this.experienceList = response.data.result.item.items
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getlablePage()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.experience {
    .experience-box {
        margin: 10px;
        .experience-list {
            background: #f0f0f0;
            padding: 0;
            li {
                padding-left: 15px;
                background: $color-wfont;
                margin: 10px 0;
                .experience-title {
                    width: 100%;
                    margin: 0 -15px;
                    font-size: $font-m;
                    padding-left: 10px;
                }
                div {
                    padding: 10px 0;
                    border-bottom: 1px solid #f0f0f0;
                }
                .border-none {
                    border: none;
                }
            }
            
        }
    }
}




</style>