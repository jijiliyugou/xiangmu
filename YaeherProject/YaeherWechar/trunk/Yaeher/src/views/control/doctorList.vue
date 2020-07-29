<template>
    <div class="doctor-list-control padding-top">
        <mt-header fixed title="医生查看">
            <mt-button  slot="right">
                <router-link :to="{path: 'control-doctor-list', query: {new: 'new'}}" class="right-white">新医生</router-link>
            </mt-button> 
        </mt-header>
        <div class="p-content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生的名字或者标签" />
                    <div @click="searchGo">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getClinicDoctorsPage"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="group2-list group-list">
                <li v-for="(item, index) in clinicDoctors" :key="index">
                    <router-link class="displayBlock" :to="{path: '/control-doctor-list', query: {id: item.id, clinic: item.clinicName}}">
                        <p class="group-title">{{item.clinicName}}({{item.clinicType | clinicSelect}})</p>
                        <p class=""><span v-for="(item1, index1) in item.lableManages" :key="index1">{{item1.lableName}}<span v-if="index1 != item.lableManages.length-1">、</span></span></p>
                        <div class="group-case">
                            <span class="avaterNub">{{item.doctorCount}}</span>
                            <i class="mint-cell-allow-right"></i>
                            <img v-if="index2<=5" v-for="(item2, index2) in item.doctorDetailList" :key="index2"  :src="`${item2.userImage}${compressHead}`" />
                        </div>
                    </router-link>
                </li>
            </ul>        
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-control">
                <img slot="icon" src="../../assets/image/question-icon.png">
                评价统计
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/doctor-list-control">
                <img slot="icon" src="../../assets/image/class-icon.png">
                医生查看
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/audit-article">
                <img slot="icon" src="../../assets/image/class-icon.png">
                文章管理
            </mt-tab-item>
            <mt-tab-item id="nav4" href="#/control-user">
                <img slot="icon" src="../../assets/image/user-icon.png">
                个人中心
            </mt-tab-item>
        </mt-tabbar>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import Star from 'components/star/star'
export default {
    components: {
      Star
    },
    filters: {
        clinicSelect (value) {
            const clinicType = ['无', '成人', '儿童']
            return clinicType[value]
        }
    },
    data () {
        return {
            keyWord: '',
            selected: 'nav2',
            clinicDoctors: [],
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true,
            compressHead: '?imageView2/q/20'
        }
    },
    methods: {
        getClinicDoctorsPage() { // 请求专业分组
            this.loading = true
            const clinicType = 0
            const maxResultCount = this.maxResultCount
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.clinicDoctorsPage({
                skipCount,
                clinicType,
                maxResultCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        let clinicDoctors1 = response.data.result.item.items
                        this.clinicDoctors = this.clinicDoctors.concat(clinicDoctors1)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                    console.log('请求医生分组失败')
                }) 
        },
        searchGo() {
            const keyWord = this.keyWord
            if(!keyWord) {
                Toast('搜索内容不能为空')
                return
            }
            this.$refs.inputText.blur()
            this.$router.push({ 
                path: '/control-doctor-list',
                query: {
                    keyWord
                }
            })
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchGo()
            } 
        },
    },
    mounted () {
        this.getClinicDoctorsPage()
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctor-list-control{
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .group-list { padding: 0 15px;
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            .displayBlock {
                display: block;
                width: 100%;
            }
            p {
                padding-bottom: 5px;
            }
            .group-title {
                font-size: $font-l;
                color: $color-afont;
                font-weight: 600;
            }
            .group-case {
                background: #e8edfe;
                height: 30px;
                padding: 10px;
                overflow: hidden;
                border-radius: 5px;
                position: relative;
                img {
                    float: left;
                    width: 30px;
                    height: 30px;
                    margin-right: 5px;
                    border-radius: 3px;
                }    
                .avaterNub {
                    position: absolute;
                    top: 20px;
                    right: 35px;
                    color: $color-cfont;
                }
            }
        }
    }
    .group2-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            display: flex;
            .doctor-a {
                display: block;
                width: 100%;
                .doctorInfo {
                    img{
                        display: block;
                        width: 60px;
                        height: 60px;
                        border-radius: 5px;
                    }
                    .flexCase {
                        padding: 0 10px; 
                        position: relative;
                        p {
                            font-size: $font-m;
                            color: $color-bfont;
                            padding-bottom: 2px;
                        }
                        .doctor-name {
                            font-size: $font-xl;
                            color: $color-afont;
                        }
                        .star-font {
                            color: $color-star;
                        }
                    }
                }  
            }
        }
    }
}    

</style>